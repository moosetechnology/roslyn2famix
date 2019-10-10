using System;
using System.Collections.Generic;
using System.Text;

namespace Fame {
    public class Tower {
        public Repository model;
        public MetaRepository metamodel;
        public MetaRepository metaMetamodel;


        private Tower(MetaRepository m3, MetaRepository m2, Repository m1) {
            this.metaMetamodel = m3;
            this.metamodel = m2;
            this.model = m1;
        }
        private Tower(MetaRepository m3) {
            this.metaMetamodel = m3;
            this.metamodel = new MetaRepository(metaMetamodel);
            this.model = new Repository(metamodel);
        }

        public Tower() : this(MetaRepository.CreateFM3()) {

        }


    }
}
