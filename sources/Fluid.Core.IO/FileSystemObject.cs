using Fluid.Core.Interfaces;
using Object = Fluid.Core.Base.Object;

namespace Fluid.Core.IO
{
    public abstract class FileSystemObject : Object, IFileSystemObject
    {
        private IFileSystemObject _parent;

        private bool _isHidden;
        private bool _isSelected;

        private string _fullName;
        private string _name;

        /// <inheritdoc />
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public bool IsHidden
        {
            get => _isHidden;
            set
            {
                if (value == _isHidden) return;
                _isHidden = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public string FullName
        {
            get => _fullName;
            set
            {
                if (value == _fullName) return;
                _fullName = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IFileSystemObject Parent
        {
            get => _parent;
            set
            {
                if (Equals(value, _parent)) return;
                _parent = value;
                OnPropertyChanged();
            }
        }
    }
}