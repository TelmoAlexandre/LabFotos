// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var expandedAccordion = false;

jQuery(document).ready(function ($) {

    init();

    getLoadingBarHtml = () => `
        <div class="ui active centered inline large loader"></div>
    `;

    // Expandir Search
    $("#expandSearchOptions").click(function () {
        $("#searchOptions").slideToggle();
        $("#expandSearchSymbol").toggle();
        $("#collapseSearchSymbol").toggle();
    });

    // Adicionar novo input para as datas de execução nos formularios dos serviços
    $("#newDataDeExecucaoInput").click(function () {
        var id = 0;
        var last = $("#dataDeExecDiv div:last-child"); // Tenta recolher o ultimo div (child)

        // Caso exista um div, recolher o valor do id e incrementar para o proximo input
        if (last.length !== 0)
        {
            id = last.data("id") + 1;
        }
        $("#dataDeExecDiv").append(
            `<div class="form-inline" id="dataExec_${id}" data-id="${id}">
                <input type="date" name="DataExecucao" class="form-control mb-2 w-75" />
                <span class="pointer form-check-inline mb-2 ml-2" onclick="deleteElement('dataExec_${id}')">
                    <i class="far fa-trash-alt ml-1"></i>
                </span>
            </div>`
        );
    });

    // Ativar o Popper.j
    $('[data-toggle="tooltip"]').tooltip();
    // Incializar componente do semantic
    $('.ui.accordion').accordion();
});

function notifyUser(type, text)
{
    new Noty({
        type: type,
        layout: 'bottomRight',
        theme: 'bootstrap-v4',
        text: text,
        timeout: 12000,
        progressBar: true
    }).show();
}

function init() {
    initModalEvents();
}

function initModalEvents() {
    loadAjaxForm = (divFormId, modalId, href) => {
        // Adicionar um loading
        $(`#${divFormId}`).html(getLoadingBarHtml);
        $(`#${modalId}`).modal({
            closable: false,
            transition: 'fade up'
        }).modal("show");
        $(`#${divFormId}`).load(`${href}`); // Carregar html para o elemento        
    };

    handleControllerResponse = (resp, divId, modalId, href) => {
        if (resp.success) {
            if (resp.id !== null) {
                href = `${href}?id=${resp.id}`;
            }
            $(`#${divId}`).load(`${href}`, function () {
                $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
            });
            $(`#${modalId}`).modal('hide');

            notifyUser('success', 'Adicionado com sucesso.');
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
        loadAjaxForm(
            'newRequerenteForm',
            'modalNovoRequerente',
            siteUrl + '/Requerentes/CreateFormAjax'
        );
    });

    // Resposta do POST do formulário do novo requerente nos serviços
    novoRequerente = (resp) =>
        handleControllerResponse(resp, 'requerentesDropbox', 'modalNovoRequerente', siteUrl + '/Servicos/RequerentesAjax');
}

toggleDivs = (div1, div2) => 
{
    $(`${div1}`).toggle();
    $(`${div2}`).toggle();
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

confirmForm = (formId) => {
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
    $(`#${divModalDetails}`).load(siteUrl + `/Requerentes/DetailsAjax/${requerenteId}?inServicos=true`, function () {
        $('[data-toggle="tooltip"]').tooltip();
    });
};

loadRequerenteEditForm = (divEdicao, idRequerente, details) =>
{
    requerenteFormsLoadAjax(divEdicao, siteUrl + `/Requerentes/Edit/${idRequerente}?detailsLink=${details}`, idRequerente);
};

requerenteFormsLoadAjax = (divFormId, href, requerenteId) =>
{
    // Adicionar um loading
    $(`#btnEdit_${requerenteId}`).addClass("loading");
    $(`#${divFormId}`).load(`${href}`, function ()
    {
        $('[data-toggle="tooltip"]').tooltip();
        $(`#btnEdit_${requerenteId}`).removeClass("loading");

        toggleDivs(`#divDetails_${requerenteId}`, `#divEdit_${requerenteId}`);
    }); // Carregar html para o elemento        
};

editRequerente = (e, divDetailsId, formEditId, requerenteId, detailsBool) => {
    e.preventDefault(); // Não deixar o form submeter

    $(`#btnSave_${requerenteId}`).addClass("loading");

    $.ajax({
        type: "POST",
        url: siteUrl + `/Requerentes/Edit/${requerenteId}`,
        data: {
            "Nome": $(`#${formEditId} #Nome`).val(),
            "Email": $(`#${formEditId} #Email`).val(),
            "Telemovel": $(`#${formEditId} #Telemovel`).val(),
            "Responsavel": $(`#${formEditId} #Responsavel`).val(),
            "__RequestVerificationToken": $(`#${formEditId} input[name='__RequestVerificationToken']`).val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success)
            {
                $(`#${divDetailsId}`).html(getLoadingBarHtml);
                $(`#${divDetailsId}`).load(siteUrl + `/Requerentes/DetailsAjax/${requerenteId}?detailsLink=${detailsBool}`, function ()
                {
                    $('[data-toggle="tooltip"]').tooltip();
                    toggleDivs(`#divDetails_${requerenteId}`, `#divEdit_${requerenteId}`);
                });

                notifyUser('success', 'Guardado com sucesso.');
            }
            else
            {
                // Caso exista modelState erros mostra-los
                $(`#divEdit_${requerenteId}`).html(resp);
            }
            $(`#btnSave_${requerenteId}`).removeClass("loading");
        }
    });
};

createTipo = () => {
    $("#btnAddTipo").addClass("loading");

    $.ajax({
        type: "POST",
        url: siteUrl + "/Tipos/Create",
        data: {
            "Tipo.Nome": $("#modalNovoTipo #Nome").val(),
            "__RequestVerificationToken": $("#modalNovoTipo input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                // Recolher os tipos selecionados
                var tipos = $("#tiposCheckboxes input[name='Tipos']").val();

                $(`#tiposCheckboxes`).load(siteUrl + `/Servicos/TiposAjax?tipos=${tipos}`, function () {

                    $("#btnAddTipo").removeClass("loading");
                    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui

                    notifyUser('success', 'Adicionado com sucesso.');

                    $(`#modalNovoTipo`).modal('hide');
                });
            } else {
                $("#btnAddTipo").removeClass("loading");
                $(`#divFormNovoTipo`).html(resp); // Mostra o formulário com os erros do modelState
            }
        },
        error: function ()
        {
            notifyUser('error', 'Erro ao adicionar.');
            $(`#modalNovoTipo`).modal('hide');
        }
    });
};

createServSolic = () => {
    $("#btnAddServSolic").addClass("loading");

    $.ajax({
        type: "POST",
        url: siteUrl + "/ServicosSolicitados/Create",
        data: {
            "ServicoSolicitado.Nome": $("#divFormNovoServSolic #Nome").val(),
            "__RequestVerificationToken": $("#divFormNovoServSolic input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                // Recolher os tipos selecionados
                var servSolic = $("#servSolicCheckboxes input[name='ServSolicitados']").val();

                $(`#servSolicCheckboxes`).load(siteUrl + `/Servicos/ServSolicAjax?servSolic=${servSolic}`, function () {

                    $("#btnAddServSolic").removeClass("loading");
                    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui

                    notifyUser('success', 'Adicionado com sucesso.');

                    $(`#modalNovoServSolic`).modal('hide'); // Esconder o modal
                });
            } else {
                $("#btnAddServSolic").removeClass("loading");
                $(`#divFormNovoServSolic`).html(resp); // Mostra o formulário com os erros do modelState
            }
        },
        error: function ()
        {
            notifyUser('error', 'Erro ao adicionar.');

            $(`#modalNovoServSolic`).modal('hide');
        }
    });
};

createMetadado = () => {
    $("#btnMetadadoAdicionar").addClass("loading");

    $.ajax({
        type: "POST",
        url: siteUrl + "/Metadados/Create",
        data:
        {
            "Metadado.Nome": $("#newMetadadoForm #Nome").val(),
            "__RequestVerificationToken": $("#modalNovoMetadado input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) 
        {
            if (resp.success) {
                // Recolher os metadados selecionados
                var metadados = $("#metadadosCB input[name='Metadados']").val();

                $(`#metadadosCB`).load(siteUrl + `/Galerias/MetadadosDropdown?metadados=${metadados}`, function () 
                {
                    $("#btnMetadadoAdicionar").removeClass("loading");
                    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
                    $(`#modalNovoMetadado`).modal('hide');
                    notifyUser('success', 'Adicionado com sucesso.');
                });
                
            }
            else 
            {
                $("#btnMetadadoAdicionar").removeClass("loading");
                $(`#newMetadadoForm`).html(resp); // Mostra o formulário com os erros do modelState
            }
        },
        error: function ()
        {
            notifyUser('error', 'Erro ao adicionar.');
            $(`#modalNovoMetadado`).modal('hide');
        }
    });
};

// ---------------------------------------------------------------- Index dos Tipos e dos Serviços Solicitados -------------------------------------------------------------------------

showEditIndex = (tipoId, controllerName) => {

    $(`#indexDetails_${tipoId} .ui.button`).addClass('loading'); // Mostra loading no butão editar

    // Pede o form. Em caso de sucesso, corre a função
    $(`#indexEdit_${tipoId}`).load(siteUrl + `/${controllerName}/Edit/${tipoId}`, function () {
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
        url: siteUrl + `/${controllerName}/Edit/${id}`,
        data: data,
        success: function (resp) {
            $(`#editFormIndex_${id} .saveButton`).removeClass('loading'); // Remove o loading no botão guardar

            if (resp.success) // Caso seja editado com sucesso
            { 
                // Carregar o details pois este foi alterado.
                $(`#indexDetails_${id}`).load(siteUrl + `/${controllerName}/Details/${id}`, function () {

                    // Esconder o edit e mostrar o details
                    $(`#indexEdit_${id}`).slideToggle();
                    $(`#indexDetails_${id}`).slideToggle();
                });

                notifyUser('success', 'Editado com sucesso.');
            }
            else
            { // Caso o modelState falhe

                $(`#indexEdit_${id}`).html(resp);
            }
        },
        error: function () 
        {
            // Esconder o edit e mostrar o details
            $(`#indexEdit_${id}`).hide();
            $(`#indexDetails_${id}`).fadeIn();

            notifyUser('error', 'Ocorreu um erro na edição.');
        }
    });
};

/*
 * PhotoSwipe 
*/

var initPhotoSwipeFromDOM = function (gallerySelector) {

    // parse slide data (url, title, size ...) from DOM elements
    // (children of gallerySelector)
    var parseThumbnailElements = function () {
        var thumbElements = document.querySelectorAll(`${gallerySelector} figure`),
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
            index;

        index = e.target.getAttribute('data-index');

        if (index >= 0) {
            // open PhotoSwipe if valid index found
            openPhotoSwipe(index, clickedGallery);
        } else {
            openPhotoSwipe(0, clickedGallery);
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

        items = parseThumbnailElements();

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