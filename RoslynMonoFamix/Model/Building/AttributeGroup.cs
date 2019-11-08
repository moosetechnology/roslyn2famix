using Fame;
using System;
using FILE;
using Dynamix;
using FAMIX;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace FAMIX {

    public class AttributeGroup : FAMIX.Entity, FAMIX.ITyped {
        List<FAMIX.Attribute> group = new List<Attribute>();
        public TypingContext TypingContext(ISymbol symbol) {
            return FAMIX.TypingContext.StructuralEntityGroup(this, symbol);
        }
        public void SetType(Type type) {
            foreach (FAMIX.StructuralEntity e in group) {
                e.declaredType = type;
            }
        }
        public void AddAllInto(FAMIX.Class FamixClass) {
            foreach (var entity in group) {
                FamixClass.AddAttribute(entity);
                entity.parentType = FamixClass;
            }
        }
        public void AddModifiers(List<string> list) {
            foreach (var entity in group) {
                entity.Modifiers.AddRange(list);
            }
        }
        public Type Type() {
            return group[0].declaredType;
        }

        public void AddAttribute(Attribute attribute) {
            group.Add(attribute);
        }
    }
}
