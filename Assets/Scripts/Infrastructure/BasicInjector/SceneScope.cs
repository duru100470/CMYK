using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BasicInjector
{
    public abstract class SceneScope : MonoBehaviour, IInstaller
    {
        public static SceneScope Instance { get; private set; } = null;
        private Container _container;

        public Container Container => _container;

        protected void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        protected void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public GameObject Instantiate(GameObject original)
        {
            var go = UnityEngine.Object.Instantiate(original);
            GameObjectInjector.InjectSingle(go, _container);

            return go;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            var go = UnityEngine.Object.Instantiate(original, position, rotation);
            GameObjectInjector.InjectSingle(go, _container);

            return go;
        }

        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            var go = UnityEngine.Object.Instantiate(original, position, rotation, parent);
            GameObjectInjector.InjectSingle(go, _container);

            return go;
        }

        public GameObject Instantiate(GameObject original, Transform parent)
        {
            var go = UnityEngine.Object.Instantiate(original, parent);
            GameObjectInjector.InjectSingle(go, _container);

            return go;
        }

        /// <summary>
        /// 상속 후 Injection을 원하는 시점에 base.Load()로 사용하면 됨
        /// </summary>
        public virtual void Load(object param = null)
        {
            var containerBuilder = new ContainerBuilder();

            InitializeContainer(containerBuilder);
            _container = containerBuilder.Build();

            var objs = SceneManager.GetActiveScene().GetRootGameObjects();
            GameObjectInjector.InjectRecursiveMany(objs.ToList(), _container);
        }

        public virtual void Unload()
        {
        }

        /// <summary>
        /// 오버라이드 해서 Container 초기화하는데 사용
        /// </summary>
        public abstract void InitializeContainer(ContainerBuilder containerBuilder);
    }
}