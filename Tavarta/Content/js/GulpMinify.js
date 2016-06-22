'use strict';

var Transform = require('readable-stream/transform');
var rs = require('replacestream');

module.exports = function(options) {
  return new Transform({
    objectMode: true,
    transform: function(file, enc, callback) {
      if (file.isNull()) {
        return callback(null, file);
      }

      function doReplace() {
        
        if (file.isStream()) {
          file.contents = file.contents.pipe(rs());
          return callback(null, file);
        }

        if (file.isBuffer()) {
          
          var temp = String(file.contents);
          
          // Ideas: https://github.com/kangax/html-minifier/blob/gh-pages/tests/minifier.js
          
          if ((options !== undefined) && options.comments) {
            // <!--    - Match the start of the comment.
            // [\s\S]* - Match anything in between.
            // ?       - Or nothing at all.
            // -->     - Match the end of the comment.
            // g       - Match globally.
            temp = temp.replace(/<!--[\s\S]*?-->/g, '');
          }
          
          if ((options === undefined) || options.razorComments) {
            // @\*     - Match the start of the comment.
            // [\s\S]* - Match anything in between.
            // ?       - Or nothing at all.
            // \*@     - Match the end of the comment.
            // g       - Match globally.
            temp = temp.replace(/@\*[\s\S]*?\*@/g, '');
          }
          
          if ((options === undefined) || options.whitespace) {
            // >           - Match the end of a tag.
            // [\s]*       - Match any white-space.
            // \<          - Match the start of a tag.
            // (?!(\/pre)) - Do not match /pre. This stops removing white space between pre tags.
            // gi          - Match globally and case insensitive.
            temp = temp.replace(/>[\s]*\<(?!(\/pre))/gi, '><');
          }
          
          file.contents = new Buffer(temp);
          
          return callback(null, file);
        }

        callback(null, file);
      }

      doReplace();
    }
  });
};