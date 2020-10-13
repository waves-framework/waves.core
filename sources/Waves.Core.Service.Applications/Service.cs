using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using ReactiveUI.Fody.Helpers;
using Waves.Core.Base;
using Waves.Core.Base.Interfaces;
using Waves.Core.Base.Interfaces.Services;

namespace Waves.Core.Service.Applications
{
    /// <summary>
    ///     Application service.
    /// </summary>
    [Export(typeof(IService))]
    public class Service : MefLoaderService<IApplication>, IApplicationService
    {
        /// <inheritdoc />
        public event ApplicationsActionsUpdatedEventHandler ApplicationsActionsUpdated;

        /// <inheritdoc />
        public override Guid Id { get; } = Guid.Parse("B709823E-22BE-4297-982B-59A90B82D977");

        /// <inheritdoc />
        public override string Name { get; set; } = "Application Loader Service";

        /// <inheritdoc />
        [Reactive]
        public ICollection<IApplicationAction> ApplicationActions { get; set; } = new ObservableCollection<IApplicationAction>();

        /// <inheritdoc />
        protected override string ObjectsName => "Applications";

        /// <inheritdoc />
        public override void Update()
        {
            UnsubscribeApplicationEvents();

            base.Update();

            SubscribeApplicationEvents();
        }

        /// <summary>
        ///     Subscribes to application events.
        /// </summary>
        private void SubscribeApplicationEvents()
        {
            if (Objects == null) return;

            foreach (var obj in Objects) obj.ActionsUpdated += OnApplicationActionsUpdated;
        }

        /// <summary>
        ///     Unsubscribes to application events.
        /// </summary>
        private void UnsubscribeApplicationEvents()
        {
            if (Objects == null) return;

            foreach (var obj in Objects) obj.ActionsUpdated -= OnApplicationActionsUpdated;
        }

        /// <summary>
        ///     Notifies when applications actions updated.
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnApplicationsActionsUpdated(ApplicationActionsUpdatedEventArgs args)
        {
            ApplicationsActionsUpdated?.Invoke(this, args);
        }

        /// <summary>
        ///     Notifies when current application actions updated.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void OnApplicationActionsUpdated(object sender, ApplicationActionsUpdatedEventArgs args)
        {
            // remove old actions
            foreach (var action in args.RemovedActions) ApplicationActions.Remove(action);

            // add new actions
            foreach (var action in args.AddedActions) ApplicationActions.Add(action);

            OnApplicationsActionsUpdated(args);
        }
    }
}