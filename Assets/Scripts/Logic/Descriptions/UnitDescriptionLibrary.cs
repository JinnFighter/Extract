using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using Logic.Components;
using UnityEngine;

namespace Logic.Descriptions
{
    [CreateAssetMenu(fileName = "UnitDescriptionLibrary", menuName = "Scriptable Objects/UnitDescriptionLibrary")]
    public class UnitDescriptionLibrary : ScriptableObject
    {
        private readonly Dictionary<string, UnitDescription> _descriptions = new Dictionary<string, UnitDescription>();
        [field: SerializeField] public List<UnitDescription> UnitDescriptions { get; set; }

        private readonly Dictionary<EPropertyType, Func<EcsEntity, IPropertyComponent>> _propertySetters = new()
        {
            { EPropertyType.Health, entity =>
            {
                var comp = new ComponentHealth();
                entity.Replace(comp);
                return comp;
            } }
        };

        public IPropertyComponent GetPropertyComponent(EPropertyType property, EcsEntity entity)
        {
            return _propertySetters[property].Invoke(entity);
        }

        public void Init()
        {
            foreach (var unitDesc in UnitDescriptions)
            {
                _descriptions.Add(unitDesc.NameId, unitDesc);
            }
        }

        public void Terminate()
        {
            _descriptions.Clear();
        }

        public UnitDescription Get(string nameId)
        {
            return _descriptions[nameId];
        }
    }
}
