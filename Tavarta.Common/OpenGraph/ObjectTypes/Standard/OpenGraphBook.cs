﻿using System;
using System.Collections.Generic;
using System.Text;
using Decision.Common.OpenGraph.Enums;
using Decision.Common.OpenGraph.Media;
using Tavarta.Common.Extentions;

namespace Decision.Common.OpenGraph.ObjectTypes.Standard
{
    /// <summary>
    /// This object represents a physical book or e-book. This object type is part of the Open Graph standard but
    /// Facebook uses the books.book object type instead which requires an ISBN number.
    /// See http://ogp.me/
    /// </summary>
    public class OpenGraphBook : OpenGraphMetadata
    {
        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphBook"/> class.
        /// </summary>
        /// <param name="title">The title of the object as it should appear in the graph.</param>
        /// <param name="image">The default image.</param>
        /// <param name="url">The canonical URL of the object, used as its ID in the graph.</param>
        public OpenGraphBook(string title, OpenGraphImage image, string url = null)
            : base(title, image, url)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the URL to the page about the author who wrote the book. This URL must contain profile meta tags <see cref="OpenGraphProfile"/>.
        /// </summary>
        public string AuthorUrl { get; set; }

        /// <summary>
        /// Gets or sets the books unique ISBN number.
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Gets the namespace of this open graph type.
        /// </summary>
        public override string Namespace { get { return "book: http://ogp.me/ns/book#"; } }

        /// <summary>
        /// Gets or sets the release date of the book.
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the tag words associated with the book.
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

        /// <summary>
        /// Gets the type of your object, e.g. "video.movie". Depending on the type you specify, other properties may also be required.
        /// </summary>
        public override OpenGraphType Type { get { return OpenGraphType.Book; } }

        #endregion

        #region Public Methods

        /// <summary>
        /// Appends a HTML-encoded string representing this instance to the <paramref name="stringBuilder"/> containing the Open Graph meta tags.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        public override void ToString(StringBuilder stringBuilder)
        {
            base.ToString(stringBuilder);

            stringBuilder.AppendMetaPropertyContentIfNotNull("book:author", this.AuthorUrl);
            stringBuilder.AppendMetaPropertyContentIfNotNull("book:isbn", this.Isbn);
            stringBuilder.AppendMetaPropertyContentIfNotNull("book:release_date", this.ReleaseDate);
            stringBuilder.AppendMetaPropertyContentIfNotNull("book:tag", this.Tags);
        }

        #endregion
    }
}
