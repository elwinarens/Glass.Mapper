using System;
using System.Collections.Generic;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Security.AccessControl;
using Version = Sitecore.Data.Version;

namespace Glass.Mapper.Sc.Tests
{
    public interface IMockItem
    {
        Item Add(string name, BranchItem branch);
        Item Add(string name, BranchId branchId);
        Item Add(string name, TemplateItem template);
        Item Add(string name, TemplateID templateID);
        void BeginEdit();
        void ChangeTemplate(TemplateItem template);
        Item Clone(Item item);
        Item Clone(ID cloneID, Database ownerDatabase);
        Item CloneTo(Item destination);
        Item CloneTo(Item destination, bool deep);
        Item CloneTo(Item destination, string name, bool deep);
        Item CopyTo(Item destination, string copyName);
        Item CopyTo(Item destination, string copyName, ID copyID, bool deep);
        void Delete();
        int DeleteChildren();
        Item Duplicate();
        Item Duplicate(string copyName);
        bool EndEdit();
        ChildList GetChildren();
        ChildList GetChildren(ChildListOptions options);
        IEnumerable<Item> GetClones();
        IEnumerable<Item> GetClones(bool processChildren);
        string GetOuterXml(bool includeSubitems);
        void MoveTo(Item destination);
        void Paste(string xml, bool changeIDs, PasteMode mode);
        Item PasteItem(string xml, bool changeIDs, PasteMode mode);
        Guid Recycle();
        int RecycleChildren();
        void Reload();
        string GetUniqueId();
        ItemAccess Access { get; }
        ItemAppearance Appearance { get; }
        ItemAxes Axes { get; }
        BranchItem Branch { get; }
        ID BranchId { get; set; }
        BranchItem[] Branches { get; }
        ChildList Children { get; }
        Database Database { get; }
        string DisplayName { get; }
        Sitecore.Data.Items.ItemEditing Editing { get; }
        bool Empty { get; }
        FieldCollection Fields { get; }
        bool HasChildren { get; }
        bool HasClones { get; }
        ItemHelp Help { get; }
        ID ID { get; }
        ItemData InnerData { get; }
        bool IsClone { get; }
        bool IsEditing { get; }
        string Key { get; }
        Language Language { get; }
        Language[] Languages { get; }
        ItemLinks Links { get; }
        ItemLocking Locking { get; }
        BranchItem Master { get; }
        ID MasterID { get; set; }
        BranchItem[] Masters { get; }
        bool Modified { get; }
        string Name { get; set; }
        ID OriginatorId { get; }
        Item Parent { get; }
        ID ParentID { get; }
        ItemPath Paths { get; }
        ItemPublishing Publishing { get; }
        ItemRuntimeSettings RuntimeSettings { get; }
        ItemSecurity Security { get; set; }
        Item Source { get; }
        ItemUri SourceUri { get; }
        ItemState State { get; }
        ItemStatistics Statistics { get; }
        object SyncRoot { get; }
        TemplateItem Template { get; }
        ID TemplateID { get; set; }
        string TemplateName { get; }
        ItemUri Uri { get; }
        Version Version { get; }
        ItemVersions Versions { get; }
        ItemVisualization Visualization { get; }
        bool IsItemClone { get; }
        Item SharedFieldsSource { get; }
        string this[string fieldName] { get; set; }
        string this[int index] { get; set; }
        string this[ID fieldID] { get; set; }
    }
}