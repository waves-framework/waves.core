using System;
using Fluid.Core.Base;
using Fluid.Core.Services;

namespace Fluid.Core.Sandbox
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var obj = new ModuleService();

            obj.PropertyChanged += OnTestObjectPropertyChanged;

            obj.Name = "Property";

            Console.ReadLine();
        }

        /// <summary>
        /// Notifies when test object's property changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnTestObjectPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName + " was changed!");
        }
    }
}