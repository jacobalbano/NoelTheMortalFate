$locator.service('$confirm', {
	instance: {
        modal: function(message) {
            return new Promise(function (resolve, reject) {
                const options = { okay: { colors: { bg: 'is-danger' } }, cancel: { colors: { bg: 'is-white'} } };
                bulmabox.confirm('Confirm', message, data => resolve(data ? 'ok' : 'cancel'), options);
            });
		}
	}
});