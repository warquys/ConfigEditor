using ConfigEditor.Controls;
using ConfigEditor.Elements;
using ConfigEditor.Factory;
using ConfigEditor.Interfaces;
using ConfigEditor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ConfigEditor.Commands
{
    public class ElementEditCommand<T> : BaseCommand<T>
    {
        #region Attributes & Properties
        private IListManager _manager;
        protected IListManager Manager { get { return _manager; } }

        private readonly Func<bool> _canExecute;
        //
        private readonly Func<T, T> _constructor;
        private readonly List<uint> _listNonExeParamValues = new List<uint> { 0 /* ??? */};

        #endregion


        #region Constructors
        public ElementEditCommand(IListManager manager, bool canExecute = true) : this(manager, (e) => e, canExecute)
        { }

        public ElementEditCommand(IListManager manager, Func<T, T> constructor, Func<bool> canExecute)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            if (constructor == null) { throw new ArgumentNullException(nameof(constructor)); }

            _manager = manager;
            _constructor = constructor;
            _canExecute = canExecute;
        }

        public ElementEditCommand(IListManager manager, Func<T, T> constructor, bool canExecute = true)
        {
            // Safe design
            if (manager == null) { throw new ArgumentNullException(nameof(manager)); }
            if (constructor == null) { throw new ArgumentNullException(nameof(constructor)); }

            _manager = manager;
            _constructor = constructor;
            _canExecute = () => canExecute;
        }

        public ElementEditCommand(IListManager manager, IEnumerable<uint> listNonExeParamValues) : this(manager)
        {
            _listNonExeParamValues.AddRange(listNonExeParamValues);
        }
        #endregion Constructors


        #region Methods
        protected override bool CanExecuteValue
        {
            get
            {
                var param = Parameter as BaseUintElement;
                if (param != null)
                {
                    if (_listNonExeParamValues.Any(nonExeVal => param.Id == nonExeVal))
                    {
                        return false;
                    }
                }
                return Parameter != null && _canExecute.Invoke();
            }
        }


        protected override void ExecuteCommand()
        {
            if (CanExecuteValue)
            {
                new DialogWaitCommand(() =>
                {
                    T loadedElement = _constructor.Invoke(Parameter);

                    EditElement(loadedElement);// _queryControl?.FindForm().Text);
                }).Execute();
            }
        }

        protected virtual void EditElement(T data)
        {
            Form frm = ECSFormUtility.FindForm<T>(data);

            if (frm is null)
            {
                CreateForm(data);
            }
            else if (!ECSFormUtility.ModalContext)
            {
                ECSFormUtility.ActivateForm(frm);
            }
            else
            {
                ECSFormUtility.UnicityError();
            }
        }

        protected virtual ECSUserControl GetDetailControl(T data)
        {
            // Safe design
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            ECSUserControl ctrlDetail;
            (_manager as IWriteManager).CurrentObject = data;
            ctrlDetail = ECSDetailFactory.CreateDetailControl(_manager as IWriteManager);

            return ctrlDetail;
        }

        private void CreateForm(T data)
        {
            // Safe design
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            ECSUserControl ctrlDetail = GetDetailControl(data);

            if (ctrlDetail != null)
            {
                if (ctrlDetail is ECSEditUserControl)
                {
                    (ctrlDetail as ECSEditUserControl).DataSourceSaved += (sender, e) =>
                    {
                        new DialogWaitCommand(() =>
                        {
                            _manager.UpdateElement(sender);
                        }).Execute();
                    };
                }
                ECSFormUtility.OpenForm<T>(ctrlDetail.Text, ctrlDetail);
            }
        }
        #endregion
    }

}