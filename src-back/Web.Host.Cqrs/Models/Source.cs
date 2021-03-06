﻿using Common.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Host.Cqrs.Models
{
    public class Source
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Адрес загрузки
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Парсер для источника
        /// </summary>
        public SourceParsers SourceParser { get; set; }
    }
}
