﻿namespace Tavarta.Common.Experimental.ViewModel
{
    /// <summary>
    /// A base class for a view's model.
    /// </summary>
    public class ViewModel<T> : Tavarta.Common.Experimental.ViewModel.ViewModel
    {
        /// <summary>
        /// Gets or sets the model value.
        /// </summary>
        public T Value { get; set; }
    }
}
