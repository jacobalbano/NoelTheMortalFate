Vue.component('translation-table', {
    props: ['strings'],
	
    data: function () {
		const dynamicItems = [];
		let set = { };

        return {
			tableSettings: {
				rowHeaders: true,
				colHeaders: ['Address', 'Meta', 'Source value', 'Patch value'] ,
				columns: [
					{ data: 'address' },
					{ data: 'instructions'},
					{ data: 'sourceValue', readOnly: true },
					{ data: 'patchValue', renderer: patchValueRenderer }
				],

				cells: function(row, col) {
					const data = this.instance.getSourceDataAtRow(row);
					const copy = _.find(data.instructions, { instructionType: 'CopyInstruction' });
					if (copy != null)
						return { readOnly: true };
				},

				stretchH: 'last',
				comments: true,
		  
				filters: true,
				dropdownMenu: false,
				hiddenColumns: {
					columns: [0, 1],
					indicators: false
				},

				contextMenu: {
					items: {
						copyFrom: {
							hidden: () => dynamicItems.length == 0,
							name: 'Copy value from:',
							disable: function() { return true; },
							submenu: {
								items: dynamicItems
							}
						}     
					}
				},

				afterLoadData,
				beforeContextMenuSetItems,

				licenseKey: 'non-commercial-and-evaluation'
			}
        };

		function afterLoadData(initialLoad) {
			set = {};

			//	get data for 'Source value' column
			const columns = this.getDataAtCol(2);
			columns.forEach((value, row) => {
				const data = this.getSourceDataAtRow(row);

				if (value in set) {
					set[value].push(data);
				} else {
					set[value] = [data];
				}
			});

			this.render();
		}

		function beforeContextMenuSetItems(menuItems) {
			const [row, col, _r, _c] = (this.getSelected()[0] || []);
			dynamicItems.splice(0, dynamicItems.length);

			if (row == _r && col == _c) {
				const data = this.getSourceDataAtRow(row);
				const strings = set[data.sourceValue];
				if (strings !== undefined) {
					let i = 0;
					for (const transStr of strings) {
						if (transStr.patchValue != null) {
							dynamicItems.push({
								key: `copyFrom:${i++}`,
								name: transStr.patchValue,
								callback: function() {
									data.instructions.push({ instructionType: 'CopyInstruction', lineReference: transStr.address });
									this.render();
								}
							})
						}
					}
				}

			}
		}
		
		function patchValueRenderer(instance, td, row, col, prop, value, cellProperties) {
			const data = instance.getSourceDataAtRow(row);
			const copy = _.find(data.instructions, x => x.instructionType === 'CopyInstruction');
			if (copy != null) {
				const referredString = _.find(instance.getSourceData(), { address : copy.lineReference });

				if (referredString != null)
					value = referredString.patchValue;
			}

			Handsontable.renderers.TextRenderer.call(this, instance, td, row, col, prop, value, cellProperties);
		}
    },
	
    components: {
        HotTable : Handsontable.vue.HotTable
    },

    template: `
	<template>
		<hot-table
			:data="strings"
			:settings="tableSettings"
		></hot-table>
	</template`
});