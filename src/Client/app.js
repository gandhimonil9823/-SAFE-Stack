// a unique random key generator
  function getUniqueId () {
    return 'private-' + Math.random().toString(36).substr(2, 9);
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
    })()

  
