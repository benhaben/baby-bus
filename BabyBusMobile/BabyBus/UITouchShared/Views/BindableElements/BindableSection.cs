using System;
using System.Collections;
using System.Collections.Generic;
using CrossUI.Touch.Dialog.Elements;
using Cirrious.CrossCore;
using UIKit;

namespace BabyBus.iOS
{
    /// <summary>
    /// Bindable section. this section support bind data to it's element, but itself don't support bind list
    /// why do not support bind:
    /// 1.I can not find a way to support binding that at the same time to support add head, add trail and update
    /// 2.I don not want to store data in the section again, because elements store them 
    /// 3.list is hard to implement two way bind, on way bind is not necessary
    /// </summary>
    public class BindableSection<TElementTemplate> : Section
        where TElementTemplate : Element, IBindableElement, new()
    {
        public BindableSection()
            : base()
        {
        }

        public BindableSection(string caption = null)
            : base(caption)
        {
        }

        public long ElementsCount()
        {
            return this.Elements.Count;
        }

        //        public bool HasUnreadElements()
        //        {
        //            return true;
        //        }

        /// <summary>
        /// Adds the rows after tail.
        /// </summary>
        /// <param name="iEnumerable">old data</param>
        public void AddRowsAfterTail(IEnumerable iEnumerable)
        {
            if (iEnumerable == null)
            {
                return;
            }
            var newElements = new List<Element>();
            if (iEnumerable != null)
            {
                try
                {
                    foreach (var item in iEnumerable)
                    {
                        var element = Activator.CreateInstance<TElementTemplate>();
                        element.DoBind();
                        element.DataContext = item;

                        newElements.Add((Element)element);
                    }
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message);
                }

                this.AddAll(newElements);

                var root = this.Parent as RootElement;

                if (root == null)
                {
                    root = this.GetImmediateRootElement();
                }

                if (root != null)
                {
                    var tableView = root.GetContainerTableView();
                    if (tableView != null)
                        tableView.ReloadData();
                }
            }
        }

        /// <summary>
        /// Adds the rows before head.
        /// </summary>
        /// <param name="iEnumerable">new data</param>
        public void AddRowsBeforeHead(IEnumerable iEnumerable)
        {
            if (iEnumerable == null)
            {
                return;
            }
            var newElements = new List<Element>();
            if (iEnumerable != null)
            {
                try
                {
                    foreach (var item in iEnumerable)
                    {
                        var element = Activator.CreateInstance<TElementTemplate>();
                        element.DoBind();
                        element.DataContext = item;

                        newElements.Add((Element)element);
                    }
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message);
                }

                this.Insert(0, UITableViewRowAnimation.None, newElements);

                var root = this.Parent as RootElement;

                if (root == null)
                {
                    root = this.GetImmediateRootElement();
                }

                if (root != null)
                {
                    var tableView = root.GetContainerTableView();
                    if (tableView != null)
                        tableView.ReloadData();
                }
            }
        }

        /// <summary>
        /// Updates the elements and reload table.
        /// </summary>
        /// <param name="iEnumerable">new data and updated data</param>
        public void UpdateElementsAndReloadTable(IEnumerable iEnumerable)
        {
            if (iEnumerable == null)
            {
                return;
            }
            else
            {
                
                var newElements = new List<Element>();

                try
                {
                    foreach (var item in iEnumerable)
                    {
                        var notInList = true;
                        foreach (var source in this.Elements)
                        {
                            var iBindableElement = source as IBindableElement;

                            if (iBindableElement.DataContext.Equals(item))
                            {
                                //update
                                iBindableElement.DataContext = item;
                                notInList = false;
                            }

                        }

                        if (notInList)
                        {
                            //insert into top
                            var element = Activator.CreateInstance<TElementTemplate>();
                            element.DoBind();
                            element.DataContext = item;
                            newElements.Add((Element)element);
                        }
                    }
                        
                }
                catch (Exception e)
                {
                    Xamarin.Insights.Report(e, Xamarin.Insights.Severity.Critical);
                    Mvx.Trace(e.Message);
                }


                this.Insert(0, UITableViewRowAnimation.None, newElements);

                var root = this.Parent as RootElement;

                if (root == null)
                {
                    root = this.GetImmediateRootElement();
                }

                if (root != null)
                {
                    var tableView = root.GetContainerTableView();
                    if (tableView != null)
                        tableView.ReloadData();
                }
            }
                
        }


        /// <summary>
        /// Init the data set.
        /// </summary>
        /// <param name="itemsSource">data to init elements</param>
        public void InitData(IEnumerable itemsSource)
        {
            var newElements = new List<Element>();
            if (itemsSource != null)
            {
                try
                {
                    foreach (var item in itemsSource)
                    {
                        var element = Activator.CreateInstance<TElementTemplate>();
                        element.DoBind();
                        element.DataContext = item;
                        newElements.Add((Element)element);
                    }
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message);
                }
            }
            else
            {
                return;
            }

            this.RemoveRange(0, Elements.Count);
            this.AddAll(newElements);

            var root = this.Parent as RootElement;

            if (root == null)
            {
                root = this.GetImmediateRootElement();
            }

            if (root != null)
            {
                var tableView = root.GetContainerTableView();
                if (tableView != null)
                    tableView.ReloadData();
            }
        }
    }
}