const select2SpanishLanguageSettings = {
    errorLoading: function () {
        return 'No se pudieron cargar los resultados.';
    },
    inputTooLong: function (args) {
        var remainingChars = args.input.length - args.maximum;
        return 'Por favor, elimine ' + remainingChars + ' caracter(es)';
    },
    inputTooShort: function (args) {
        var remainingChars = args.minimum - args.input.length;
        return 'Por favor, introduzca ' + remainingChars + ' o más caracteres';
    },
    loadingMore: function () {
        return 'Cargando más resultados...';
    },
    maximumSelected: function (args) {
        return 'Sólo puede seleccionar ' + args.maximum + ' elemento(s)';
    },
    noResults: function () {
        return 'No se encontraron resultados';
    },
    searching: function () {
        return 'Buscando...';
    }
};

export function initSelect2(){
    const staticDropdowns = [$('#categoryName')];
    
    staticDropdowns.forEach(function (d) {
        d.select2({
            placeholder: 'Todos',
            minimumInputLength: 1,
            language: select2SpanishLanguageSettings,
        });
    })

    // $('#dropdown').select2({
    //     ajax: {
    //         url: '/Submissions/Create',
    //         dataType: 'json',
    //         delay: 100,
    //         data: function (params) {
    //             return {
    //                 handler: 'FetchStores',
    //                 partialName: params.term
    //             };
    //         },
    //         processResults: function (data) {
    //             return {
    //                 results: data
    //             };
    //         },
    //         cache: true
    //     },
    //     placeholder: 'Seleccionar',
    //     minimumInputLength: 1,
    //     language: select2SpanishLanguageSettings
    // });
}



