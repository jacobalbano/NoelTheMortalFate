Vue.component('translation-table', {
    props: ['strings'],
	
    data: function () {
		const vm = this;
		const referenceMenuItems = [];
		let groupBySrcVal = { };

        return {
			tableSettings: {
				rowHeaders: true,
				colHeaders: ['Source value', 'Patch value'],
				columns: [
					{ data: 'sourceValue', readOnly: true },
					{ data: 'patchValue', renderer: patchValueRenderer }
				],

				cells: function(row, col) {
					const data = vm.strings[row];
					const hot = this.instance;
					const cellProps = { };
					
					const copy = _.find(data.instructions, { instructionType: 'CopyInstruction' });

					if (col == 0) {
						const group = groupBySrcVal[data.sourceValue];
						if (group && group.length > 1 && data.patchValue == null && copy == null) {
							cellProps.className = 'attention';
							cellProps.comment = { value: 'Possible duplicate; consider referencing another string instead of repeating data', readOnly: true };
						} else {
							cellProps.className = '';
							cellProps.comment = { };
						}
					}
					if (col == 1) {
						cellProps.readOnly = copy != null;
					}
					
					return cellProps;
				},

				stretchH: 'last',
				comments: true,
				dropdownMenu: false,

				contextMenu: {
					callback: function() { this.render(); },
					items: {
						'cut': { },
						'copy': { },
						'undo': { },
						'redo': { },
						'---------': { },

						referTo: {
							hidden: () => referenceMenuItems.length == 0,
							name: 'Reference other string',
							submenu: { items: referenceMenuItems }
						},

						referToThis: {
							name: 'Make duplicates refer here',
							hidden: function() {
								const cell = selectedCellOrNull(this);
								if (cell == null)
									return true;

								const data = vm.strings[cell.row];
								const dupes = groupBySrcVal[data.sourceValue];

								return dupes.length <= 1;
							},

							callback: function() {
								const cell = selectedCellOrNull(this);
								const data = vm.strings[cell.row];
								const dupes = groupBySrcVal[data.sourceValue];

								for (const str of dupes) {
									if (str == data) continue;
									const existingCopy = _.find(str.instructions, { instructionType: 'CopyInstruction' });
									_.pull(str.instructions, existingCopy);
									str.instructions.push({
										instructionType: 'CopyInstruction',
										lineReference: data.address
									});
								}
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
			console.log('afterLoadData');
			groupBySrcVal = {};
			for (const str of vm.strings) {
				const group = groupBySrcVal[str.sourceValue] || (groupBySrcVal[str.sourceValue] = []);
				group.push(str);
			}
		}

		function beforeContextMenuSetItems(menuItems) {
			referenceMenuItems.splice(0, referenceMenuItems.length);
			const cell = selectedCellOrNull(this);
			const data = vm.strings[cell.row];

			if (cell != null) {
				addReferenceOptions(data);
			}

			function addReferenceOptions(data) {
				const existingCopy = _.find(data.instructions, { instructionType: 'CopyInstruction' });
				if (existingCopy != null)
					referenceMenuItems.push({
						key: 'referTo:removeReference',
						name: 'Remove reference',
						callback: () => _.pull(data.instructions, existingCopy)
					});

				_.forEach(groupBySrcVal[data.sourceValue], (transStr, i) => {
					if (transStr !== data && transStr.patchValue != null) {
						referenceMenuItems.push({
							key: `referTo:${i}`,
							name: `"${transStr.patchValue}"`,

							callback: function() {
								_.pull(data.instructions, existingCopy);
								data.instructions.push({
									instructionType: 'CopyInstruction',
									lineReference: transStr.address
								});
							}
						})
					}
				});
			}
		}
		
		function patchValueRenderer(instance, td, row, col, prop, value, cellProperties) {
			const data = vm.strings[row];
			const copy = _.find(data.instructions, { instructionType : 'CopyInstruction' });
			if (copy != null) {
				const referredString = _.find(instance.getSourceData(), { address : copy.lineReference });

				if (referredString != null)
					value = referredString.patchValue;
			}

			Handsontable.renderers.TextRenderer.call(this, instance, td, row, col, prop, value, cellProperties);
		}

		function selectedCellOrNull(self) {
			const [row, col, _r, _c] = (self.getSelected()[0] || [1, 2, 3, 4]);
			if (row == _r && col == _c)
				return { row, col };

			return null;
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