using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Managers;
using Sitecore.Data;
using Sitecore;
using Sitecore.Data.Engines;
using Sitecore.Data.Events;
using Sitecore.Events;
using Sitecore.Publishing;

namespace Glass.Mapper.Sc.Caching
{
    public class PublishEndCacheUpdate
    {
        private const string LastUpdate = "GlassCacheLastUpdateTime";

        /// <summary>
        /// Locals the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Local(object sender, EventArgs args)
        {
            var sitecoreEventArgs = args as SitecoreEventArgs;
            
            if (sitecoreEventArgs == null) return;
            
            var publisher = sitecoreEventArgs.Parameters[0] as Publisher;
            
            if (publisher != null) CacheWorker(publisher.Options.TargetDatabase);
        }

        /// <summary>
        /// Remotes the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public void Remote(object sender, EventArgs args)
        {
            //get the database and date range to process
            var sitecoreEventArgs = args as PublishEndRemoteEventArgs;
            
            if (sitecoreEventArgs == null) return;
            
            var database = Sitecore.Configuration.Factory.GetDatabase(sitecoreEventArgs.TargetDatabaseName);
            CacheWorker(database);
        }

        /// <summary>
        /// Updates the index.
        /// </summary>
        /// <param name="database">The database.</param>
        private void CacheWorker(Database database)
        {
            var startTime = LastUpdateTime(database);
            var endTime = DateTime.UtcNow;

            //build the list used to make sure we don't process the save item event twice
            var performedActions = new List<String>();

            var entries = HistoryManager.GetHistory(database, startTime, endTime);
            if (entries.Count > 0)
            {
                var templateList = new List<string>();
                var cache = Context.Default.ObjectCacheConfiguration.ObjectCache;

                foreach (var entry in entries)
                {
                    //get the item action log string
                    var logString = String.Format("{0}-{1}", entry.ItemId, entry.Action);

                    //we only want to process save and delete events
                    //updates are also processed as a save event
                    if (!performedActions.Contains(logString) && (entry.Action == HistoryAction.Deleted || entry.Action == HistoryAction.Saved))
                    {
                        performedActions.Add(logString);
                    }

                    var item = database.GetItem(entry.ItemId);

                    if (item == null) continue;
                    
                    var templaetId = item.TemplateID.ToString();
                    if (templateList.All(x => x != templaetId))
                        templateList.Add(templaetId);
                }

                //clear the related caches after the index has finished updating
                foreach (var teamplet in templateList)
                {
                    cache.ClearRelatedCache(teamplet);
                }
            }

            //update the last update time
            database.Properties[LastUpdate + Environment.MachineName] = DateUtil.ToIsoDate(endTime, true);
        }

        private static DateTime LastUpdateTime(Database database)
        {
            var lastUpdate = database.Properties[LastUpdate + Environment.MachineName];
            return DateUtil.ParseDateTime(lastUpdate, DateTime.UtcNow.AddMinutes(-2));
        }

    }
}
