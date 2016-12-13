using System.Collections.ObjectModel;
using System.IO;

namespace SimpleImageGallery {

    public class PhotoCollection : ObservableCollection<Photo> {
        DirectoryInfo _directory;

        public PhotoCollection() { }

        public PhotoCollection(string path) : this(new DirectoryInfo(path)) { }

        public PhotoCollection(DirectoryInfo directory) {
            _directory = directory;
            Update();
        }

        public string Path {
            set {
                _directory = new DirectoryInfo(value);
                Update();
            }
            get { return _directory.FullName; }
        }

        public DirectoryInfo Directory {
            set {
                _directory = value;
                Update();
            }
            get { return _directory; }
        }

        private void Update() {
            this.Clear();
            foreach (FileInfo f in _directory.GetFiles("*.*"))  {
                if (f.FullName.EndsWith(".jpg") || 
                    f.FullName.EndsWith(".png") ||
                    f.FullName.EndsWith(".bmp"))
                    Add(new Photo(f.FullName));
            }
        }

    }

}