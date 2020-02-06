using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {

    public class StructuralEntityGroup : FAMIX.Entity, FAMIX.ITyped {
        List<FAMIX.StructuralEntity> group = new List<StructuralEntity>();
        public TypingContext TypingContext(ISymbol symbol) {
            return FAMIX.TypingContext.StructuralEntityGroup(this, symbol);
        }
        public void SetType(Type type) {
            foreach (FAMIX.StructuralEntity e in group) {
                e.declaredType = type;
            }
        }
        public void AddAllAttributesInto(FAMIX.Class FamixClass) {
            foreach (var entity in group) {
                FamixClass.AddAttribute((Attribute)entity);
                ((Attribute)entity).parentType = FamixClass;
            }
        }

        public void AddAllLocalVariablesInto(FAMIX.BehaviouralEntity FamixEntity) {
            foreach (var entity in group) {
                FamixEntity.AddLocalVariable((LocalVariable)entity);
                ((LocalVariable)entity).parentBehaviouralEntity = FamixEntity;
            }
        }
        public void AddModifiers(List<string> list) {
            foreach (var entity in group) {
                entity.Modifiers.AddRange(list);
            }
        }
        public Type Type() {
            if (group.Count > 0) {
                return group[0].declaredType;
            }
            return null;
        }

        public void AddAttribute(Attribute attribute) {
            group.Add(attribute);
        }

        public void AddLocalVariable(LocalVariable variable) {
            group.Add(variable);
        }

    }
}
