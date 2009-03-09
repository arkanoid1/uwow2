using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Hazzik.Data.Xml {
	public class XmlDao<T> : IDao<T> {
		#region Fields

		private readonly FileInfo _file;
		private readonly XmlSerializer _serializer;
		protected List<T> _entities = new List<T>();

		#endregion

		#region ctors

		protected XmlDao(string filename) {
			_serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(filename + "s"));
			_file = new FileInfo(string.Format(@"..\..\..\{0}.xml", filename));
			if(_file.Exists) {
				using(var s = _file.Open(FileMode.Open, FileAccess.Read)) {
					_entities = (List<T>)_serializer.Deserialize(s);
				}
			}
		}

		#endregion

		#region IDao<T> Members

		public void Delete(T entity) {
			_entities.Remove(entity);
		}

		public void Save(T entity) {
			if(!_entities.Contains(entity)) {
				_entities.Add(entity);
			}
		}

		public void SubmitChanges() {
			using(var s = _file.Open(FileMode.Create, FileAccess.Write)) {
				_serializer.Serialize(s, _entities);
			}
		}

		#endregion
	}
}