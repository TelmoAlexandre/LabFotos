// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var expandedAccordion = false;

jQuery(document).ready(function ($) {

    init();

    getLoadingBarHtml = () => `
        <div class="ui active centered inline large loader"></div>
    `;

    // Expandir Search Servicos
    $("#expandSearchOptions").click(function () {
        $("#searchOptions").slideToggle();
        $("#expandSearchSymbol").toggle();
        $("#collapseSearchSymbol").toggle();
    });

    // Adicionar novo input para as datas de execução nos formularios dos serviços
    $("#newDataDeExecucaoInput").click(function () {
        var count = $("#dataDeExecDiv div").length;
        $("#dataDeExecDiv").append(
            `<div class="form-inline" id="dataExec_${count}">
                <input type="date" name="DataExecucao" class="form-control mb-2 w-75" />
                <span class="pointer form-check-inline mb-2 ml-2" onclick="deleteElement('dataExec_${count}')">
                    <i class="far fa-trash-alt ml-1"></i>
                </span>
            </div>`
        );
    });

    // Ativar o Popper.j
    $('[data-toggle="tooltip"]').tooltip();
    // Incializar componente do semantic
    $('.ui.dropdown').dropdown();
    $('.ui.accordion').accordion();
});

function init() {
    initModalEvents();
}

function initModalEvents() {
    servicoFormsLoadAjax = (divFormId, modalId, href) => {
        // Adicionar um loading
        $(`#${divFormId}`).html(getLoadingBarHtml);
        $(`#${modalId}`).modal({
            transition: 'fade up'
        }).modal("show");
        $(`#${divFormId}`).load(`${href}`); // Carregar html para o elemento        
    };

    requerenteFormsLoadAjax = (divFormId, href) => {
        // Adicionar um loading
        $(`#${divFormId}`).html(getLoadingBarHtml);
        $(`#${divFormId}`).load(`${href}`, function () {
            $('[data-toggle="tooltip"]').tooltip();
        }); // Carregar html para o elemento        
    };

    handleControllerResponse = (resp, divId, modalId, href) => {
        if (resp.success) {
            $(`#${divId}`).load(`${href}`, function () {
                $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
            });
            $(`#${modalId}`).modal('hide');
            // Notificação 'Noty'
            new Noty({
                type: 'success',
                layout: 'bottomRight',
                theme: 'bootstrap-v4',
                text: 'Adicionado com sucesso.',
                timeout: 4000,
                progressBar: true
            }).show();
        }
    };

    //Trocar serviços na página de detalhes do requerente
    $('.ui .item').on('click', function () {
        $('.ui .item').removeClass('active');
        $(this).addClass('active');
    });

    // Modal do novo requerente com fetch do formulário em Ajax
    $("#btnModalNovoRequerente").click(function (e) {
        e.preventDefault();

        // Loading equanto dá fetch ao formulário
        servicoFormsLoadAjax(
            'newRequerenteForm',
            'modalNovoRequerente',
            '/Requerentes/CreateFormAjax'
        );
    });

    editarRequerenteIndex = (divEdicao, divRequerente, idRequerente) => {
        $(`#${divRequerente}`).shape('flip back');
        $(`#${divEdicao}`).width('290px');
        $(`#${divEdicao}`).height('291.1px');
        requerenteFormsLoadAjax(`${divEdicao}`, `/Requerentes/EditIndex/${idRequerente}`);
    };

    editarRequerenteDetails = (divEdicao, divRequerente, idRequerente) => {
        $(`#${divRequerente}`).shape('flip back');
        $(`#${divEdicao}`).width('290px');
        $(`#${divEdicao}`).height('291.1px');
        requerenteFormsLoadAjax(`${divEdicao}`, `/Requerentes/EditDetails/${idRequerente}`);
    };

    // Modal do novo tipo com fetch do formulário em Ajax
    $("#btnModalNovoTipo").click(function (e) {
        e.preventDefault();
        // Loading equanto dá fetch ao formulário
        servicoFormsLoadAjax(
            'divFormNovoTipo',
            'modalNovoTipo',
            '/Tipos/Create'
        );
    });
    // Modal do novo tipo com fetch do formulário em Ajax
    $("#btnModalNovoServSolic").click(function (e) {
        e.preventDefault();
        // Loading equanto dá fetch ao formulário
        servicoFormsLoadAjax(
            'divFormNovoServSolic',
            'modalNovoServSolic',
            '/ServicosSolicitados/Create'
        );
    });

    // Resposta do POST do formulário do novo requrente
    novoRequerente = (resp) =>
        handleControllerResponse(resp, 'requerentesDropbox', 'modalNovoRequerente', '/Servicos/RequerentesAjax');

    // Resposta do POST do formulário do novo tipo
    novoTipo = (resp) =>
        handleControllerResponse(resp, 'tiposCheckboxes', 'modalNovoTipo', '/Servicos/TiposAjax');

    // Resposta do POST do formulário do novo tipo
    novoServSolic = (resp) =>
        handleControllerResponse(resp, 'servSolicCheckboxes', 'modalNovoServSolic', '/Servicos/ServSolicAjax');
}

flipCard = (shapeId, transition) => {
    $(`#${shapeId}`).shape(`${transition}`);
};

// Submit do form das pesquisas dos requerentes
requerentesSubmitSearchForm = (pageReq) => {
    $("#pageReqNum").val(pageReq);
    $("#requerentesIndexSearchForm").submit();
};

hideModal = () => {
    $(".ui.modal").modal("hide");
};

showModal = (modalId) => {
    $(`#${modalId}`).modal("show");
};

deleteElement = (id) => {
    $(`#${id}`).remove();
};

// Modais de confirmação

confirmForm = (formId, divCardId) => {
    $("#confirmarSubmit").modal({
        closable: false,
        onDeny: function () {
            $("#confirmarSubmit").modal('hide');
        },
        onApprove: function () {
            $(`#${formId}`).submit();
        },
        transition: 'scale'
    }).modal('show');
};

modalCancel = (id) => {
    $(`#${id}`).modal({
        closable: false,
        onDeny: function () {
            $(`#${id}`).modal('hide');
        },
        transition: 'scale'
    }).modal('show');
};

servicoRequerenteDetails = (divModalDetails, requerenteId) => {
    $(`#${divModalDetails}`).modal('show');
    $(`#${divModalDetails}`).html(getLoadingBarHtml);
    $(`#${divModalDetails}`).load(`/Requerentes/DetailsAjax/${requerenteId}`, function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
};

// Detalhes do serviço com o id que recebe por parametro
// e envia para o div com o id que recebe por parametro uma partialView com os detalhes do serviço
requerenteServicoDetails = (divId, servicoId) => {
    $(`#${divId}`).html($(`#loadingServico`));
    $(`#loadingServico`).removeAttr('hidden');
    $(`#${divId}`).load(`/Servicos/DetailsAjax/${servicoId}`);
};

createTipoOnServicosEdit = (e, idServico) => {
    e.preventDefault(); // Não deixar o form submeter

    $.ajax({
        type: "POST",
        url: "/Tipos/Create",
        data: {
            "Tipo.Nome": $("#Tipo_Nome").val(),
            "__RequestVerificationToken": $("#modalNovoTipo input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#tiposCheckboxes`).load(`/Servicos/TiposAjax/${idServico}`, function () {
                    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
                });
                $(`#modalNovoTipo`).modal('hide');
                $("#Tipo_Nome").val(null); // Limpa o campo do formulário
                // Notificação 'Noty'
                new Noty({
                    type: 'success',
                    layout: 'bottomRight',
                    theme: 'bootstrap-v4',
                    text: 'Adicionado com sucesso.',
                    timeout: 4000,
                    progressBar: true
                }).show();
            }
        }
    });
};

requerenteEditFormSubmitIndex = (e, divRequerente, divDetailsId, formEditId, requerenteId) => {
    e.preventDefault(); // Não deixar o form submeter

    $.ajax({
        type: "POST",
        url: `/Requerentes/EditIndex/${requerenteId}`,
        data: {
            "Nome": $(`#${formEditId} #Nome`).val(),
            "Email": $(`#${formEditId} #Email`).val(),
            "Telemovel": $(`#${formEditId} #Telemovel`).val(),
            "Responsavel": $(`#${formEditId} #Responsavel`).val(),
            "__RequestVerificationToken": $(`#${formEditId} input[name='__RequestVerificationToken']`).val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#${divDetailsId}`).html(getLoadingBarHtml);
                $(`#${divRequerente}`).shape('flip over');
                $(`#${divDetailsId}`).load(`/Requerentes/DetailsIndexAjax/${requerenteId}`, function () {
                    $('[data-toggle="tooltip"]').tooltip();
                });
                // Notificação 'Noty'
                new Noty({
                    type: 'success',
                    layout: 'bottomRight',
                    theme: 'bootstrap-v4',
                    text: 'Guardado com sucesso.',
                    timeout: 4000,
                    progressBar: true
                }).show();
            }
        }
    });
};

requerenteEditFormSubmitDetails = (e, divRequerente, divDetailsId, formEditId, requerenteId) => {
    e.preventDefault(); // Não deixar o form submeter

    $.ajax({
        type: "POST",
        url: `/Requerentes/EditDetails/${requerenteId}`,
        data: {
            "Nome": $(`#${formEditId} #Nome`).val(),
            "Email": $(`#${formEditId} #Email`).val(),
            "Telemovel": $(`#${formEditId} #Telemovel`).val(),
            "Responsavel": $(`#${formEditId} #Responsavel`).val(),
            "__RequestVerificationToken": $(`#${formEditId} input[name='__RequestVerificationToken']`).val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#${divDetailsId}`).html(getLoadingBarHtml);
                $(`#${divRequerente}`).shape('flip over');
                $(`#${divDetailsId}`).load(`/Requerentes/DetailsIndexAjaxDetails/${requerenteId}`, function () {
                    $('[data-toggle="tooltip"]').tooltip();
                });
                // Notificação 'Noty'
                new Noty({
                    type: 'success',
                    layout: 'bottomRight',
                    theme: 'bootstrap-v4',
                    text: 'Guardado com sucesso.',
                    timeout: 4000,
                    progressBar: true
                }).show();
            }
        }
    });
};

createServSolicOnServicosEdit = (e, idServico) => {
    e.preventDefault(); // Não deixar o form submeter

    $.ajax({
        type: "POST",
        url: "/ServicosSolicitados/Create",
        data: {
            "ServicoSolicitado.Nome": $("#ServicoSolicitado_Nome").val(),
            "__RequestVerificationToken": $("#divFormNovoServSolic input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#servSolicCheckboxes`).load(`/Servicos/ServSolicAjax/${idServico}`, function () {
                    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
                });
                $(`#modalNovoServSolic`).modal('hide'); // Esconder o modal
                $("#ServicoSolicitado_Nome").val(null); // Limpa o campo do formulário
                // Notificação 'Noty'
                new Noty({
                    type: 'success',
                    layout: 'bottomRight',
                    theme: 'bootstrap-v4',
                    text: 'Adicionado com sucesso.',
                    timeout: 4000,
                    progressBar: true
                }).show();
            }
        }
    });
};

// ---------------------------------------------------------------- Index dos Tipos e dos Serviços Solicitados -------------------------------------------------------------------------

showEditIndex = (tipoId, controllerName) => {

    $(`#indexDetails_${tipoId} .ui.button`).addClass('loading'); // Mostra loading no butão editar

    // Pede o form. Em caso de sucesso, corre a função
    $(`#indexEdit_${tipoId}`).load(`/${controllerName}/Edit/${tipoId}`, function () {
        $(`#indexDetails_${tipoId}`).slideToggle(); // Esconde o details
        $(`#indexDetails_${tipoId} .ui.button`).removeClass('loading'); // Remove loading no botão editar
        $(`#indexEdit_${tipoId}`).slideToggle(); // Mostra o form editar

        // Focar o input no ultimo caracter
        var val = $(`#indexEdit_${tipoId} input[type="text"]`).val();
        $(`#indexEdit_${tipoId} input[type="text"]`).focus().val('').val(val);

    });
};

cancelEditIndex = (tipoId) => {
    // Esconder o edit e mostrar o details
    $(`#indexEdit_${tipoId}`).slideToggle();
    $(`#indexDetails_${tipoId}`).slideToggle();
};

submitEditIndex = (e, id, controllerName, fieldName) => {
    e.preventDefault(); // Não deixar o form submeter, pois o request vai ser feito por Ajax

    $(`#editFormIndex_${id} .saveButton`).addClass('loading'); // Loading no botão guardar

    // Preencher a data dinamicamente
    var data = {
        "__RequestVerificationToken": $(`#editFormIndex_${id} input[name='__RequestVerificationToken']`).val(),
        "X-Requested-With": "XMLHttpRequest"
    };
    data[`${fieldName}.ID`] = $(`#editFormIndex_${id} #ID`).val(); // Caso o fieldName seja Tipo, fica "Tipo.ID": ...
    data[`${fieldName}.Nome`] = $(`#editFormIndex_${id} #Nome`).val();

    $.ajax({
        type: "POST",
        url: `/${controllerName}/Edit/${id}`,
        data: data,
        success: function (resp) {
            $(`#editFormTiposIndex_${id} .saveButton`).removeClass('loading'); // Remove o loading no botão guardar

            if (resp.success) { // Caso seja editado com sucesso

                // Carregar o details pois este foi alterado.
                $(`#indexDetails_${id}`).load(`/${controllerName}/Details/${id}`, function () {

                    // Esconder o edit e mostrar o details
                    $(`#indexEdit_${id}`).slideToggle();
                    $(`#indexDetails_${id}`).slideToggle();
                });

                // Notificação 'Noty'
                new Noty({
                    type: 'success',
                    layout: 'bottomRight',
                    theme: 'bootstrap-v4',
                    text: 'Editado com sucesso.',
                    timeout: 4000,
                    progressBar: true
                }).show();

            } else { // Caso o modelState falhe

                $(`#indexEdit_${id}`).html(resp);
            }
        },
        error: function () {

            // Esconder o edit e mostrar o details
            $(`#indexEdit_${id}`).hide();
            $(`#indexDetails_${id}`).fadeIn();

            // Notificação 'Noty'
            new Noty({
                type: 'error',
                layout: 'bottomRight',
                theme: 'bootstrap-v4',
                text: 'Ocorreu um erro na edição.',
                timeout: 4000,
                progressBar: true
            }).show();
        }
    });
};

/*
 * PhotoSwipe 
*/

var initPhotoSwipeFromDOM = function (gallerySelector) {

    // parse slide data (url, title, size ...) from DOM elements
    // (children of gallerySelector)
    var parseThumbnailElements = function (el) {
        var thumbElements = el.childNodes,
            numNodes = thumbElements.length,
            items = [],
            figureEl,
            linkEl,
            item;

        for (var i = 0; i < numNodes; i++) {

            figureEl = thumbElements[i]; // <figure> element

            // include only element nodes
            if (figureEl.nodeType !== 1) {
                continue;
            }

            linkEl = figureEl.children[0]; // <a> element

            // create slide object
            item = {
                src: linkEl.getAttribute('href'),
                w: 0,
                h: 0
            };

            if (figureEl.children.length > 1) {
                // <figcaption> content
                item.title = figureEl.children[1].innerHTML;
            }

            if (linkEl.children.length > 0) {
                // <img> thumbnail element, retrieving thumbnail url
                item.msrc = linkEl.children[0].getAttribute('src');
            }

            item.el = figureEl; // save link to element for getThumbBoundsFn

            items.push(item);
        }

        return items;
    };

    // find nearest parent element
    var closest = function closest(el, fn) {
        return el && (fn(el) ? el : closest(el.parentNode, fn));
    };

    // triggers when user clicks on thumbnail
    var onThumbnailsClick = function (e) {
        e = e || window.event;
        e.preventDefault ? e.preventDefault() : e.returnValue = false;

        var eTarget = e.target || e.srcElement;

        // find root element of slide
        var clickedListItem = closest(eTarget, function (el) {
            return (el.tagName && el.tagName.toUpperCase() === 'FIGURE');
        });

        if (!clickedListItem) {
            return;
        }

        // find index of clicked item by looping through all child nodes
        // alternatively, you may define index via data- attribute
        var clickedGallery = clickedListItem.parentNode,
            childNodes = clickedListItem.parentNode.childNodes,
            numChildNodes = childNodes.length,
            nodeIndex = 0,
            index;

        for (var i = 0; i < numChildNodes; i++) {
            if (childNodes[i].nodeType !== 1) {
                continue;
            }

            if (childNodes[i] === clickedListItem) {
                index = nodeIndex;
                break;
            }
            nodeIndex++;
        }

        if (index >= 0) {
            // open PhotoSwipe if valid index found
            openPhotoSwipe(index, clickedGallery);
        }
        return false;
    };

    // parse picture index and gallery index from URL (#&pid=1&gid=2)
    var photoswipeParseHash = function () {
        var hash = window.location.hash.substring(1),
            params = {};

        if (hash.length < 5) {
            return params;
        }

        var vars = hash.split('&');
        for (var i = 0; i < vars.length; i++) {
            if (!vars[i]) {
                continue;
            }
            var pair = vars[i].split('=');
            if (pair.length < 2) {
                continue;
            }
            params[pair[0]] = pair[1];
        }

        if (params.gid) {
            params.gid = parseInt(params.gid, 10);
        }

        return params;
    };

    var openPhotoSwipe = function (index, galleryElement, disableAnimation, fromURL) {
        var pswpElement = document.querySelectorAll('.pswp')[0],
            gallery,
            options,
            items;

        items = parseThumbnailElements(galleryElement);

        // define options (if needed)
        options = {

            // define gallery index (for URL)
            galleryUID: galleryElement.getAttribute('data-pswp-uid'),
            errorMsg: "Ocorreu um erro ao carregar a imagem."
        };

        // PhotoSwipe opened from URL
        if (fromURL) {
            if (options.galleryPIDs) {
                // parse real index when custom PIDs are used
                // http://photoswipe.com/documentation/faq.html#custom-pid-in-url
                for (var j = 0; j < items.length; j++) {
                    if (items[j].pid === index) {
                        options.index = j;
                        break;
                    }
                }
            } else {
                // in URL indexes start from 1
                options.index = parseInt(index, 10) - 1;
            }
        } else {
            options.index = parseInt(index, 10);
        }

        // exit if index not found
        if (isNaN(options.index)) {
            return;
        }

        if (disableAnimation) {
            options.showAnimationDuration = 0;
        }

        // Pass data to PhotoSwipe and initialize it
        gallery = new PhotoSwipe(pswpElement, PhotoSwipeUI_Default, items, options);
        gallery.listen('gettingData', function (index, item) {
            if (item.w < 1 || item.h < 1) { // unknown size
                var img = new Image();
                img.onload = function () { // will get size after load
                    item.w = this.width; // set image width
                    item.h = this.height; // set image height
                    gallery.invalidateCurrItems(); // reinit Items
                    gallery.updateSize(true); // reinit Items
                };
                img.src = item.src; // let's download image
            }
        });
        gallery.init();
    };

    // loop through all gallery elements and bind events
    var galleryElements = document.querySelectorAll(gallerySelector);

    for (var i = 0, l = galleryElements.length; i < l; i++) {
        galleryElements[i].setAttribute('data-pswp-uid', i + 1);
        galleryElements[i].onclick = onThumbnailsClick;
    }

    // Parse URL and open gallery if it contains #&pid=3&gid=1
    var hashData = photoswipeParseHash();
    if (hashData.pid && hashData.gid) {
        openPhotoSwipe(hashData.pid, galleryElements[hashData.gid - 1], true, true);
    }
};

//download.js v4.2, by dandavis; 2008-2016. [CCBY2] see http://danml.com/download.html for tests/usage
// v1 landed a FF+Chrome compat way of downloading strings to local un-named files, upgraded to use a hidden frame and optional mime
// v2 added named files via a[download], msSaveBlob, IE (10+) support, and window.URL support for larger+faster saves than dataURLs
// v3 added dataURL and Blob Input, bind-toggle arity, and legacy dataURL fallback was improved with force-download mime and base64 support. 3.1 improved safari handling.
// v4 adds AMD/UMD, commonJS, and plain browser support
// v4.1 adds url download capability via solo URL argument (same domain/CORS only)
// v4.2 adds semantic variable names, long (over 2MB) dataURL support, and hidden by default temp anchors
// https://github.com/rndme/download

(function (root, factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as an anonymous module.
        define([], factory);
    } else if (typeof exports === 'object') {
        // Node. Does not work with strict CommonJS, but
        // only CommonJS-like environments that support module.exports,
        // like Node.
        module.exports = factory();
    } else {
        // Browser globals (root is window)
        root.download = factory();
    }
}(this, function () {

    return function download(data, strFileName, strMimeType) {

        var self = window, // this script is only for browsers anyway...
            defaultMime = "application/octet-stream", // this default mime also triggers iframe downloads
            mimeType = strMimeType || defaultMime,
            payload = data,
            url = !strFileName && !strMimeType && payload,
            anchor = document.createElement("a"),
            toString = function (a) { return String(a); },
            myBlob = (self.Blob || self.MozBlob || self.WebKitBlob || toString),
            fileName = strFileName || "download",
            blob,
            reader;
        myBlob = myBlob.call ? myBlob.bind(self) : Blob;

        if (String(this) === "true") { //reverse arguments, allowing download.bind(true, "text/xml", "export.xml") to act as a callback
            payload = [payload, mimeType];
            mimeType = payload[0];
            payload = payload[1];
        }


        if (url && url.length < 2048) { // if no filename and no mime, assume a url was passed as the only argument
            fileName = url.split("/").pop().split("?")[0];
            anchor.href = url; // assign href prop to temp anchor
            if (anchor.href.indexOf(url) !== -1) { // if the browser determines that it's a potentially valid url path:
                var ajax = new XMLHttpRequest();
                ajax.open("GET", url, true);
                ajax.responseType = 'blob';
                ajax.onload = function (e) {
                    download(e.target.response, fileName, defaultMime);
                };
                setTimeout(function () { ajax.send(); }, 0); // allows setting custom ajax headers using the return:
                return ajax;
            } // end if valid url?
        } // end if url?


        //go ahead and download dataURLs right away
        if (/^data\:[\w+\-]+\/[\w+\-]+[,;]/.test(payload)) {

            if (payload.length > (1024 * 1024 * 1.999) && myBlob !== toString) {
                payload = dataUrlToBlob(payload);
                mimeType = payload.type || defaultMime;
            } else {
                return navigator.msSaveBlob ?  // IE10 can't do a[download], only Blobs:
                    navigator.msSaveBlob(dataUrlToBlob(payload), fileName) :
                    saver(payload); // everyone else can save dataURLs un-processed
            }

        }//end if dataURL passed?

        blob = payload instanceof myBlob ?
            payload :
            new myBlob([payload], { type: mimeType });


        function dataUrlToBlob(strUrl) {
            var parts = strUrl.split(/[:;,]/),
                type = parts[1],
                decoder = parts[2] === "base64" ? atob : decodeURIComponent,
                binData = decoder(parts.pop()),
                mx = binData.length,
                i = 0,
                uiArr = new Uint8Array(mx);

            for (i; i < mx; ++i) uiArr[i] = binData.charCodeAt(i);

            return new myBlob([uiArr], { type: type });
        }

        function saver(url, winMode) {

            if ('download' in anchor) { //html5 A[download]
                anchor.href = url;
                anchor.setAttribute("download", fileName);
                anchor.className = "download-js-link";
                anchor.innerHTML = "downloading...";
                anchor.style.display = "none";
                document.body.appendChild(anchor);
                setTimeout(function () {
                    anchor.click();
                    document.body.removeChild(anchor);
                    if (winMode === true) { setTimeout(function () { self.URL.revokeObjectURL(anchor.href); }, 250); }
                }, 66);
                return true;
            }

            // handle non-a[download] safari as best we can:
            if (/(Version)\/(\d+)\.(\d+)(?:\.(\d+))?.*Safari\//.test(navigator.userAgent)) {
                url = url.replace(/^data:([\w\/\-\+]+)/, defaultMime);
                if (!window.open(url)) { // popup blocked, offer direct download:
                    if (confirm("Displaying New Document\n\nUse Save As... to download, then click back to return to this page.")) { location.href = url; }
                }
                return true;
            }

            //do iframe dataURL download (old ch+FF):
            var f = document.createElement("iframe");
            document.body.appendChild(f);

            if (!winMode) { // force a mime that will download:
                url = "data:" + url.replace(/^data:([\w\/\-\+]+)/, defaultMime);
            }
            f.src = url;
            setTimeout(function () { document.body.removeChild(f); }, 333);

        }//end saver




        if (navigator.msSaveBlob) { // IE10+ : (has Blob, but not a[download] or URL)
            return navigator.msSaveBlob(blob, fileName);
        }

        if (self.URL) { // simple fast and modern way using Blob and URL:
            saver(self.URL.createObjectURL(blob), true);
        } else {
            // handle non-Blob()+non-URL browsers:
            if (typeof blob === "string" || blob.constructor === toString) {
                try {
                    return saver("data:" + mimeType + ";base64," + self.btoa(blob));
                } catch (y) {
                    return saver("data:" + mimeType + "," + encodeURIComponent(blob));
                }
            }

            // Blob but not URL support:
            reader = new FileReader();
            reader.onload = function (e) {
                saver(this.result);
            };
            reader.readAsDataURL(blob);
        }
        return true;
    }; /* end download() */
}));