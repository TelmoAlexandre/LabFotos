// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var expandedAccordion = false;

jQuery(document).ready(function ($) {

    init();

    getLoadingBarHtml = () => `
        <div class="ui active centered inline massive loader"></div>
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

    $("#searchTiposCB").load("/Servicos/TiposAjax");
    $("#searchServSolicCB").load("/Servicos/ServSolicAjax");

    // Ativar o Popper.j
    $('[data-toggle="tooltip"]').tooltip();
    // Incializar componente do semantic
    $('.ui.dropdown').dropdown();
});

function init() {
    initModalEvents();
}

function initModalEvents()
{
    servicoFormsLoadAjax = (divFormId, modalId, href) => {
        // Adicionar um loading
        $(`#${divFormId}`).html(getLoadingBarHtml);
        $(`#${modalId}`).modal({
            transition: 'fade up'
        }).modal("show");
        $(`#${divFormId}`).load(`${href}`); // Carregar html para o elemento        
    };

    handleControllerResponse = (resp, divId, modalId, href) => {
        if (resp.success) {
            $(`#${divId}`).load(`${href}`);
            $(`#${modalId}`).modal('toggle');
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

// Submit do form das pesquisas dos requerentes
requerentesSubmitSearchForm = (pageReq) => {
    $("#pageReqNum").val(pageReq);
    $("#requerentesIndexSearchForm").submit();
};

// Limpar pesquisas dos servicos
clearServicosSearch = () => {
    $(`#searchParams input[type="search"]`).val("");
    $(`#searchParams input[type="date"]`).val("");
    $(`#searchParams input[type="checkbox"]`).prop("checked", false);
};

hideModal = () => {
    $(".ui.modal").modal("hide");
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
resetAccordionCollapse = () => {
    if (expandedAccordion) {
        $(".accordionBody").addClass("show");
    }
};

deleteElement = (id) => {
    $(`#${id}`).remove();
};

// Modal de confirmação
showConfirmationDialog = (flag, param, message) => {
    switch (flag) {
        case 'submitForm':
            bootbox.confirm({
                animate: true,
                message: message,
                buttons: {
                    confirm: { label: '<i class="fa fa-check"></i> Sim', className: 'btn-primary' },
                    cancel: { label: '<i class="fa fa-times"></i> Não', className: 'btn-secondary' }
                },
                callback: function (result) {
                    if (result) {
                        $(`#${param}`).submit();
                    }
                }
            });
            break;
        case 'unsavedChanges':
            bootbox.confirm({
                animate: true,
                message: "Existem alterações que não foram guardadas. Tem a certeza que prentede sair?",
                buttons: {
                    confirm: { label: '<i class="fa fa-check"></i> Sim', className: 'btn-primary' },
                    cancel: { label: '<i class="fa fa-times"></i> Não', className: 'btn-secondary' }
                },
                callback: function (result) {
                    if (result) {
                        window.location.href = param;
                    }
                }
            });
            break;
        case 'ServicosDetailsDelete':
            bootbox.confirm({
                animate: true,
                message: "Tem a certeza que prentende apagar este serviço?",
                buttons: {
                    confirm: { label: '<i class="fa fa-check"></i> Sim', className: 'btn-danger'},
                    cancel: { label: '<i class="fa fa-times"></i> Não', className: 'btn-primary'}
                },
                callback: function (result) {
                    if (result) {
                        $("#servicosDetailsDeleteForm").submit();
                    }
                }
            });
            break;
        default: break;
    }
};

servicoRequerenteDetails = (divModalDetails, divDetailsId, requerenteId) => {
    $(`#${divModalDetails}`).modal('toggle');
    $(`#${divDetailsId}`).html(getLoadingBarHtml);
    $(`#${divDetailsId}`).load(`/Requerentes/DetailsAjax/${requerenteId}`);
};

// Detalhes do serviço com o id que recebe por parametro
// e envia para o div com o id que recebe por parametro uma partialView com os detalhes do serviço
requerenteServicoDetails = (divId, servicoId) => {
        $(`#${divId}`).html(getLoadingBarHtml);
        $(`#${divId}`).load(`/Servicos/DetailsAjax/${servicoId}`);
};

createTipoOnServicosEdit = (idServico) => {
    $.ajax({
        type: "POST",
        url: "/Tipos/Create",
        data: {
            "Tipos.Nome": $("#Tipos_Nome").val(),
            "__RequestVerificationToken": $("#modalNovoTipo input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#tiposCheckboxes`).load(`/Servicos/TiposAjax/${idServico}`);
                $(`#modalNovoTipo`).modal('toggle');
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

createServSolicOnServicosEdit = (idServico) => {
    $.ajax({
        type: "POST",
        url: "/ServicosSolicitados/Create",
        data: {
            "ServicosSolicitados.Nome": $("#ServicosSolicitados_Nome").val(),
            "__RequestVerificationToken": $("#divFormServicosSolicitados input[name='__RequestVerificationToken']").val(),
            "X-Requested-With": "XMLHttpRequest"
        },
        success: function (resp) {
            if (resp.success) {
                $(`#servSolicCheckboxes`).load(`/Servicos/ServSolicAjax/${idServico}`);
                $(`#modalNovoServSolicitado`).modal('toggle');
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