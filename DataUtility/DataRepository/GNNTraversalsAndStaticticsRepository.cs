using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataUtility.DataRepository
{
    public class GNNTraversalsAndStaticticsRepository
    {
        #region Fields
        private readonly ITextFileReadUoW _textFileReadUoW;
        #endregion

        #region CTor
        public GNNTraversalsAndStaticticsRepository(ITextFileReadUoW textFileReadUoW)
        {
            _textFileReadUoW = textFileReadUoW;
        }
        #endregion

        #region Methods        
        #endregion
    }
}
