using System;
using System.Collections.Generic;
using System.Text;

namespace MusicConverterTest
{
    interface IGoodBrother
    {
        /// <summary>
        /// Updates all information
        /// </summary>
        void Update();

        /// <summary>
        /// Check if this Good Brother is valid
        /// </summary>
        /// <returns></returns>
        bool CheckValidity();

        /// <summary>
        /// Export this Good Brother
        /// </summary>
        /// <returns></returns>
        string Compose();
    }
}
