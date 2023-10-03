using FinancialBird.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application
{
    public interface IBuchungsReader
    {
        public Buchung[] Read(string file);
    }
}
