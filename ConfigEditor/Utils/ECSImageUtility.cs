using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using ConfigEditor.Properties;
using DevExpress.Utils;

namespace ConfigEditor.Utils
{
    public static class ECSImageUtility
    {
        #region Methods

        public static Icon GetIcon(string name)
        {
            return GetIcon(name, 16);
        }

        private static IDictionary<string, Icon> _dicIcon = new Dictionary<string, Icon>();
        private static IDictionary<string, Bitmap> _dicBitmap = new Dictionary<string, Bitmap>();

        public static Icon GetIcon(string name, int size = 24)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }

            string nameIco = $"{name}_{size}";
            if (_dicIcon.ContainsKey(nameIco))
            {
                return _dicIcon[nameIco];
            }

            string resxName = SpecialName(name);

            var resxIcon = Resources.ResourceManager.GetObject(resxName) as Icon;

            if (resxIcon != null)
            {
                _dicIcon[nameIco] = new Icon(resxIcon, size, size);
                return _dicIcon[nameIco];
            }

            Bitmap imageResx = GetImage(resxName, 24);

            if (imageResx != null)
            {
                _dicIcon[nameIco] = new Icon(Icon.FromHandle(imageResx.GetHicon()), size, size);
                return _dicIcon[nameIco];
            }
            _dicIcon[nameIco] = null;
            return null;
        }

        public static Bitmap GetImage(string name)
        {
            return GetImage(name, 24);
        }

        public static Bitmap GetImage(string name, int size = 24)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(name)) return null;

            string nameIco = $"{name}_{size}";
            if (_dicBitmap.ContainsKey(nameIco))
            {
                return _dicBitmap[nameIco];
            }
            string resxName = SpecialName(name);
            var resxIcon = Resources.ResourceManager.GetObject(resxName) as Icon;
            if (resxIcon != null)
            {
                _dicBitmap[nameIco] = new Icon(resxIcon, size, size).ToBitmap();
                return _dicBitmap[nameIco];
            }

            string internalName = String.Concat(resxName, "_", size);
            var bMap = Resources.ResourceManager.GetObject(internalName) as Bitmap;
            if (bMap != null)
            {
                _dicBitmap[nameIco] = bMap;
                return _dicBitmap[nameIco];
            }

            _dicBitmap[nameIco] = Resources.ResourceManager.GetObject(resxName) as Bitmap;
            return _dicBitmap[nameIco];
        }

        private static string SpecialName(string name)
        {
            // Safe design
            if (String.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException(nameof(name)); }

            string resxName = name.ToUpperInvariant();

            if (resxName.StartsWith("ACN"))
            {
                return resxName.Replace("ACN", "ICN_ACTION");
            }
            else
            {
                return resxName;
            }
        }


        private static List<ImageInfo> _listImgInfo;

        /// <summary>
        /// Retrieves a collection of images from the application resources
        /// </summary>
        public static List<ImageInfo> GetImgFromResources()
        {
            if (_listImgInfo is null)
            {
                _listImgInfo = Resources.ResourceManager
                   .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                   .Cast<DictionaryEntry>()
                   .Where(x => x.Value is Bitmap || x.Value is Icon)
                   .Select(x => GetImageInfo(x.Key.ToString(), x.Value))
                   .ToList();
            }
            return _listImgInfo;
        }

        private static ImageInfo GetImageInfo(string key, object value)
        {
            if (value is Bitmap)
            {
                return new ImageInfo(key, (Image)value);
            }

            if (value is Icon)
            {
                var icn = value as Icon;
                return new ImageInfo(key, icn.ToBitmap());
            }

            throw new Exception("Only objects Icon and Bitmap are supported!");
        }

        private static ImageCollection _imagesFromRes;
        /// <summary>
        /// Gets static collection of Icon and Bitmap images from the project resources
        /// </summary>
        public static ImageCollection ImagesFromRes
        {
            get
            {
                if (_imagesFromRes is null)
                {
                    _imagesFromRes = new ImageCollection();

                    var list = Resources.ResourceManager
                        .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                        .Cast<DictionaryEntry>()
                        .Where(x => x.Value is Bitmap || x.Value is Icon).ToList();

                    foreach (DictionaryEntry dicEntry in list)
                    {
                        _imagesFromRes.AddImage(GetImage(dicEntry), dicEntry.Key.ToString());
                    }

                }

                return _imagesFromRes;
            }
        }


        private static Image GetImage(DictionaryEntry dicEntry)
        {
            if (dicEntry.Value is Bitmap)
            {
                return (Image)dicEntry.Value;
            }

            if (dicEntry.Value is Icon)
            {
                var icn = dicEntry.Value as Icon;
                if (icn != null)
                {
                    return icn.ToBitmap();
                }
            }

            throw new Exception("Only objects Icon and Bitmap are supported!");
        }


        #endregion Methods
    }
}