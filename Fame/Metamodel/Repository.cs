﻿using Fame.Fm3;
using Fame.Internal;
using Fame.Parser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fame {
    public class Repository {

        private IList elements;

        public MetaRepository metamodel;

        public IList GetElements() {
            return elements;
        }

        /**
		 * Creates an empty tower of models. The tower has three layers: both this
		 * and the meta-layer are initially empty, whereas the topmost layer is
		 * initialized with a new FM3 package.
		 * 
		 */
        public Repository() : this(new MetaRepository(MetaRepository.CreateFM3())) {

        }

        /**Creates a empty layer with the given meta-layer.

		* <p>
		* If the specified parameter is <code>null</code>, creates a
		* self-describing layer(ie an meta-metamodel).
		* 
		* @param metamodel
		*/



        public Repository(MetaRepository metamodel) {
            // allow null in order to boot-strap self-described meta-models
            this.metamodel = metamodel;
            this.elements = new List<object>();
        }

        public T New<T>(string qname) {
            Element m = metamodel.Get(qname);

            if (m != null) {
                if (m.GetType() == typeof(MetaDescription)) {
                    T element = (T)((MetaDescription)m).NewInstance();
                    this.Add(element);
                    return element;
                }
            } else {

                throw new Exception("WTF");
            }

            return default(T);//null
        }

        internal MetaDescription DescriptionOf(object each) {
            return metamodel.GetDescription(each.GetType());
        }

        public virtual void Add(object element) {
            MetaDescription meta = metamodel.GetDescription(element.GetType());

            elements.Add(element);


            foreach (PropertyDescription property in meta.AllAttributes()) {
                if (!property.IsPrimitive()) {
                    Boolean isRoot = property.Type.IsRoot();
                    foreach (object value in property.ReadAll(element)) {
                        if (!(isRoot &&
                                (value is String ||
                                value is Boolean ||
                                Number.IsNumber(value)))) {
                            try {
                                this.Add(value);
                            } catch (ClassNotMetadescribedException e) {
                                throw new ElementInPropertyNotMetadescribed(property);
                            }
                        }
                    }
                }
            }

        }

        public void AddAll(IList<object> all) {
            foreach (object o in all) Add(o);
        }

        public void Accept(MSEPrinter visitor) {
            RepositoryVisitor rv = new RepositoryVisitor(this, visitor);
            rv.Run();
        }

        public string ExportMSEFile(string filename) {
            var msePrinter = new MSEPrinter(filename);
            Accept(msePrinter);
            return msePrinter.GetMSE();
        }
    }
}
