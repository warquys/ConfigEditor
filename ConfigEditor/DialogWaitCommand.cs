using System;
using DevExpress.XtraSplashScreen;

namespace ConfigEditor
{
    /// <summary>
    /// Command to execute the action passed into the constructor
    /// </summary>
    public class DialogWaitCommand
    {
        #region Attributes & Properties

        private readonly Action _command;
        private Exception _exception;

        #endregion

        #region Constructors

        /// <summary>
        /// Executes the action passed in as a parameter while displaying a "Please wait..." form
        /// </summary>
        public DialogWaitCommand(Action command)
        {
            // Safe design
            if (command == null) { throw new ArgumentNullException(nameof(command)); }

            _command = command;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays 'Please wait', executes Action, hides 'Please Wait'
        /// </summary>
        public void Execute()
        {
            _exception = null;
            bool close = (SplashScreenManager.Default == null);
            if (close)
            {
                SplashScreenManager.ShowDefaultWaitForm();
                SplashScreenManager.Default.SetWaitFormCaption("Please wait...");
                SplashScreenManager.Default.SetWaitFormDescription("Loading ...");
            }
#if !DEBUG
			try
			{
				_command.Invoke();
			}
			catch (Exception e)
			{
				_exception = e;
			}
			if (SplashScreenManager.Default != null && close)
			{
				SplashScreenManager.CloseForm();
			}
			if (_exception != null)
			{
				throw _exception;
			}
#else
            _command.Invoke();
            if (SplashScreenManager.Default != null && close)
            {
                SplashScreenManager.CloseForm();
            }
#endif
        }

        #endregion Constructors

    }
}
