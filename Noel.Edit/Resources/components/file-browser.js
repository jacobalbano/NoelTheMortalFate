$locator.run(['$api', '$workspace'], function($api, $workspace) {
	Vue.component('file-browser', {
		props: ['files'],
		data: function () {
			return {
				number: 0,
				filetree: [],
				fileFilter: '',
				modal: null
			};
		},
	
		created: async function() {
			this.filetree = await $api.getFiletree();
		},

		methods: {
			openModal: function() {
				const self = this;
				const modal = this.modal = {
					term: this.fileFilter,
					loading: false,
					files: [],

					refresh,
					close
				};

				modal.refresh();

				async function refresh() {
					modal.loading = true;
					modal.files = await $api.getFullTextSearch(modal.term);
					modal.loading = false;
				}

				function close(file) {
					if (file != null)
						$workspace.openFile(file.seasonNum, file.filename);
				
					self.modal = null;
				}
			}
		},

		template: `
		<div class="flex-panel">
			<p class="title is-5">File Browser</p>
			<form @submit.prevent="openModal()">
				<p class="control has-icons-left">
					<input class="input" type="text" v-model="fileFilter" placeholder="Search">
					<span class="icon is-left">
						<i class="fas fa-search" aria-hidden="true"></i>
					</span>
				</p>
				<p>
					<button type="submit" class="button is-fullwidth is-link">
						Search in all files
					</button>
				</p>
			</form>
			<hr>
			<aside class="menu">
				<ul class="menu-list">
					<season-folder v-for="folder in filetree"
						:number="folder.number"
						:files="folder.files"
						:filter="fileFilter"
					></season-folder>
				</ul>
			</aside>

			<div v-if="modal != null" class="modal" :class="{ 'is-active': modal != null }">
				<div @click="modal.close()" class="modal-background"></div>
				<div class="modal-card" style="width: 50% !important">
					<header class="modal-card-head">
						<p class="modal-card-title">Full-text search</p>
						<button @click="modal.close()" class="delete" aria-label="close"></button>
					</header>
					<section class="modal-card-body" >
						<form @submit.prevent="modal.refresh()">
							<p class="control has-icons-left">
								<input @submit="modal.refresh()" class="input" type="text" v-model="modal.term" placeholder="Search">
								<span class="icon is-left">
									<i class="fas fa-search" aria-hidden="true"></i>
								</span>
							</p>
						</form>
						<hr>
						<div v-if="modal.loading" class="center">
							Loading
							</br>
							<progress class="progress is-small is-primary" max="100">15%</progress>	
						</div>
						<div v-else-if="modal.files.length == 0" class="center">
							No files matching search term
						</div>
						<div v-else>
							<table class="table">
								<tr v-for="file in modal.files">
									<td><a @click="modal.close(file)">Season {{file.seasonNum}}/{{file.filename}}</a></td>
									<td>{{ file.match }}</td>
								</tr>
							</table>
						</div>
					</section>
				</div>
			</div>
		</div>`
	});
});