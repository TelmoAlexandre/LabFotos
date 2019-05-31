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

    // As dropdowns dos tipos e dos serviços solicitados são carregadas para um div de backup
    // Em seguida, são colocadas no devido sitio. Quando for necessário limpar pesquisa, basta
    // recuperar o html dos divs de backup
    $("#tiposBackup").load("/Servicos/TiposAjax", function () {
        $("#searchTiposCB").html($("#tiposBackup").html());
        $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
    });
    $("#servSolicBackup").load("/Servicos/ServSolicAjax", function () {
        $("#searchServSolicCB").html($("#servSolicBackup").html());
        $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
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

    editarRequerente = (divEdicao, divRequerente, idRequerente, details) => {
        $(`#${divRequerente}`).shape('flip back');
        requerenteFormsLoadAjax(`${divEdicao}`, `/Requerentes/Edit/${idRequerente}?details=${details}`);
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

// Submit do form das pesquisas dos servicos
submitSearchForm = (page) => {
    $("#pageNum").val(page);
    $("#servicosIndexSearchForm").submit();
};

flipCard = (shapeId, transition) => {
    $(`#${shapeId}`).shape(`${transition}`);
};

// Submit do form das pesquisas dos requerentes
requerentesSubmitSearchForm = (pageReq) => {
    $("#pageReqNum").val(pageReq);
    $("#requerentesIndexSearchForm").submit();
};

// Limpar pesquisas dos servicos
clearServicosSearch = () => {
    $(`#searchParams input[type="search"]`).val("");
    $(`#searchParams input[type="date"]`).val("");
    // Recarregar as dropdows dos tipos e dos serviços solicitados
    // Estes encontram-se num div de backup, pois tem o layout do semantic-ui
    // Assim evita-se alterar o codigo do semantic-ui, pois isso pode levantar problemas
    $("#searchTiposCB").html($("#tiposBackup").html());
    $("#searchServSolicCB").html($("#servSolicBackup").html());
    $('.ui.dropdown').dropdown(); // Activar as dropdows do semantic-ui
    submitSearchForm();
};

hideModal = () => {
    $(".ui.modal").modal("hide");
};

showModal = (modalId) => {
    $(`#${modalId}`).modal("show");
};

// Expandir e collapsar os accordions do Index dos serviços
$("#accordionExpand").click(function (e) {
    e.preventDefault();
    $(".accordionBody").addClass("show");
    $(".accordionOptions").toggle();
    expandedAccordion = true;
});
$("#accordionCollapse").click(function (e) {
    e.preventDefault();
    $(".accordionBody").removeClass("show");
    $(".accordionOptions").toggle();
    expandedAccordion = false;
});
resetAccordionCollapse = (resp) => {
    if (expandedAccordion) {
        $(".accordionBody").addClass("show");
    }
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

requerenteEditFormSubmit = (e, divRequerente, divDetailsId, formEditId, requerenteId, details) => {
    e.preventDefault(); // Não deixar o form submeter
    $.ajax({
        type: "POST",
        url: `/Requerentes/Edit/${requerenteId}`,
        data: {
            "Nome": $(`#${formEditId} #Nome`).val(),
            "Email": $(`#${formEditId} #Email`).val(),
            "Telemovel": $(`#${formEditId} #Telemovel`).val(),
            "Responsavel": $(`#${formEditId} #Responsavel`).val(),
            "details": details,
            "__RequestVerificationToken": $(`#${formEditId} input[name='__RequestVerificationToken']`).val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#${divDetailsId}`).html(getLoadingBarHtml);
                $(`#${divRequerente}`).shape('flip over');
                $(`#${divDetailsId}`).load(`/Requerentes/DetailsCardAjax/${requerenteId}?details=${details}`, function () {
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