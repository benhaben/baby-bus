using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;


using BabyBus.Logic.Shared;

using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using System.Collections.Generic;


namespace BabyBus.Logic.Shared
{
    public class ChildrenViewModel : BaseViewModel
    {
        private readonly IRemoteService _service;

        public ChildrenViewModel()
        {
            _service = Mvx.Resolve<IRemoteService>();
        }

        /// <summary>
        ///     Selected Checkout Object, Convert To Json for Detail Page
        /// </summary>
        public string SelectedCheckoutJson { get; set; }

        #region Property

        private List<ChildModel> children = new List<ChildModel>();

        public List<ChildModel> Children
        {
            get { return children; }
            set
            {
                children = value;
                RaisePropertyChanged(() => Children);
            }
        }

        #endregion

        #region Command

        private IMvxCommand showDetailCommand;

        public IMvxCommand ShowDetailCommand
        {
            get
            {
                showDetailCommand = showDetailCommand ??
                new MvxCommand(
                    () => ShowViewModel<ChildInfoViewModel>(
                        new {json = SelectedCheckoutJson}));
                return showDetailCommand;
            }
        }

        #endregion

        public override void InitData()
        {
            ViewModelStatus = new ViewModelStatus(UIConstants.LOADING, true, MessageType.Information, TipsType.DialogProgress);

            try
            {
                Children = _service.GetChildren().Result;
                ViewModelStatus = new ViewModelStatus(UIConstants.LOAD_SUCCESS, false, MessageType.Success, TipsType.DialogDisappearAuto);
            }
            catch (Exception ex)
            {
                ViewModelStatus = new ViewModelStatus(UIConstants.WEB_EXCEPTION, false, MessageType.Error, TipsType.DialogDisappearAuto);
            }
            finally
            {
            }
           
        }
    }
}