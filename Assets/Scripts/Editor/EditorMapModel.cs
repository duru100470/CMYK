using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEditor
{
    public class EditorMapModel : IMapModel
    {
        private List<MapObject> _objectList = new();
        private ReactiveProperty<ColorType> _bgColor = new ReactiveProperty<ColorType>();
        public ReactiveProperty<ColorType> BackgroundColor => _bgColor;

        public void AddMapObject(MapObject mapObject)
        {
            _objectList.Add(mapObject);
        }

        public void RemoveMapObject(MapObject mapObject)
        {
            _objectList.Remove(mapObject);
        }

        /// <summary>
        /// 맵 특정 좌표의 오브젝트 탐색
        /// </summary>
        /// <returns>탐색 성공 여부</returns>
        public bool TryGetObject(Coordinate dir, out MapObject obj, bool ignoreColor = false)
        {
            var target = _objectList.FirstOrDefault(obj => obj.Coordinate == dir);
            obj = null;

            if (target == default)
                return false;

            obj = target;
            return true;
        }
        public IEnumerable<MapObject> GetObjectsByInfo(ObjectInfo info, bool ignoreColor = false)
        {
            IEnumerable<MapObject> objects = _objectList.Where(obj => obj.Info.Type == info.Type);
            
            if(!ignoreColor)
                objects = objects.Where(obj => obj.Info.Color == info.Color);
            
            return objects;
        }

        public void OnColorEventOccurred(ColorChangeEvent colorChangeEvent)
        {
            foreach (MapObject rock in _objectList)
            {
                if (rock.Info.Type == ObjectType.Rock)
                {
                    foreach (MapObject overLapO in _objectList)
                    {
                        if (rock.Coordinate.Equals(overLapO.Coordinate) && (rock != overLapO))
                        {
                            RemoveMapObject(overLapO);
                            break;
                        }
                    }
                }
            }
        }
    }
}
