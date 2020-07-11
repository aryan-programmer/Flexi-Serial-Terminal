// From https://stackoverflow.com/a/3531935

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Flexi_Serial_Terminal {
	/// <summary>
	///     A command whose sole purpose is to
	///     relay its functionality to other
	///     objects by invoking delegates. The
	///     default return value for the CanExecute
	///     method is 'true'.
	/// </summary>
	public class RelayCommand : ICommand {

		#region Constructors

		/// <summary>
		///     Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute = null) {
			this.execute    = execute ?? throw new ArgumentNullException(nameof(execute));
			this.canExecute = canExecute;
		}

		#endregion // Constructors

		#region Fields

		private readonly Action<object>    execute;
		private readonly Predicate<object> canExecute;

		#endregion // Fields

		#region ICommand Members

		[DebuggerStepThrough] public bool CanExecute(object parameters) => canExecute?.Invoke(parameters) ?? true;

		public event EventHandler CanExecuteChanged {
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public void Execute(object parameters) => execute(parameters);

		#endregion // ICommand Members

	}
}