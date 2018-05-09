// Helper methods for instantiating new editor page

// a unique random key generator
function getUniqueId () {
  // url for private channel
  // return 'private-' + Math.random().toString(36).substr(2, 9);

  // url for public channel
  return Math.random().toString(36).substr(2, 9);
}

// function to get a query param's value
function getUrlParameter(name) {
  name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
  var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
  var results = regex.exec(location.search);
  return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

(function () {

  var doc = document.getElementById('doc');
  doc.contentEditable = true;
  doc.focus();

  var id = getUrlParameter('id');
  if (!id) {
    location.search = location.search
      ? '&id=' + getUniqueId() : 'id=' + getUniqueId();
    return;
  }

  // TODO: modify Promise to connect to a public (vs. private) channel
  return new Promise(function (resolve, reject) {
    // logging info
    Pusher.logToConsole = true;

    // subscribe to the changes via Pusher
    var pusher = new Pusher("a72e95caf97b5941ca0c",
                            {cluster:"us2"});
    var channel = pusher.subscribe(id);
    channel.bind('text-edit', function(html) {
      // save the current position
      var currentCursorPosition = getCaretCharacterOffsetWithin(doc);
      doc.innerHTML = html;
      // set the previous cursor position
      setCaretPosition(doc, currentCursorPosition);
    });
    channel.bind('pusher:subscription_succeeded', function() {
      resolve(channel);
    });
  }).then(function (channel) {
    function triggerChange (e) {
      channel.trigger('text-edit', e.target.innerHTML);
    }

    doc.addEventListener('input', triggerChange);
  })

  // Helper methods for text editor

  function getCaretCharacterOffsetWithin(element) {
    var caretOffset = 0;
    var doc = element.ownerDocument || element.document;
    var win = doc.defaultView || doc.parentWindow;
    var sel;
    if (typeof win.getSelection != "undefined") {
      sel = win.getSelection();
      if (sel.rangeCount > 0) {
        var range = win.getSelection().getRangeAt(0);
        var preCaretRange = range.cloneRange();
        preCaretRange.selectNodeContents(element);
        preCaretRange.setEnd(range.endContainer, range.endOffset);
        caretOffset = preCaretRange.toString().length;
      }
    } else if ( (sel = doc.selection) && sel.type != "Control") {
      var textRange = sel.createRange();
      var preCaretTextRange = doc.body.createTextRange();
      preCaretTextRange.moveToElementText(element);
      preCaretTextRange.setEndPoint("EndToEnd", textRange);
      caretOffset = preCaretTextRange.text.length;
    }
    return caretOffset;
  }

  function setCaretPosition(el, pos) {
    // Loop through all child nodes
    for (var node of el.childNodes) {
      if (node.nodeType == 3) { // we have a text node
        if (node.length >= pos) {
            // finally add our range
            var range = document.createRange(),
                sel = window.getSelection();
            range.setStart(node,pos);
            range.collapse(true);
            sel.removeAllRanges();
            sel.addRange(range);
            return -1; // we are done
        } else {
          pos -= node.length;
        }
      } else {
        pos = setCaretPosition(node,pos);
        if (pos == -1) {
            return -1; // no need to finish the for loop
        }
      }
    }
    return pos; // needed because of recursion stuff
  }
})();

  
