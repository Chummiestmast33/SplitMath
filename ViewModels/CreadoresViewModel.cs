using SliptMath.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SliptMath.ViewModels
{
    class CreadoresViewModel
    {
        //propiedades
        public ObservableCollection<Creador> PantallasCreadores { get; set; } = new ObservableCollection<Creador>();
        public CreadoresViewModel()
        {
            PantallasCreadores.Add(new Creador
            {
                Nombre = "Pablo Cortez Rodriguez",
                ImgName = "plablo.jpg",
                Funcion = "Lider de proyecto / coder"
            });
            PantallasCreadores.Add(new Creador
            {
                Nombre = "Lander Medina Mora",
                ImgName = "lander.jpg",
                Funcion = "Diseñador de interfaz"
            });
            PantallasCreadores.Add(new Creador
            {
                Nombre = "Kalep Niño Rangel",
                ImgName = "kalep.jpg",
                Funcion = "Creador de los problemas"
            });
            PantallasCreadores.Add(new Creador
            {
                Nombre = "David Zamora",
                ImgName = "david.jpg",
                Funcion = "Creador de los problemas"
            });
            PantallasCreadores.Add(new Creador
            {
                Nombre = "Diego Salazar",
                ImgName = "diego.jpg",
                Funcion = "Diseño del soundtrack"
            });
        }
    }
}
