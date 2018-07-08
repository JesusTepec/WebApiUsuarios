using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiUsuarios.Models
{
    public class Usuario
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string UserName { set; get; }
        public string Password;
    }
}
