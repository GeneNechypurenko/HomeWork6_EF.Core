using HomeWork6.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork6.ViewModels
{
    public struct ItemView : IShow<int>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
