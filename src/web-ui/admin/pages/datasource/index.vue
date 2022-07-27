<template>
  <v-card>
    <v-card-title>
      <v-icon>mdi-database-outline</v-icon>{{ $t('Datasources') }}
      <v-spacer></v-spacer>
      <v-text-field append-icon="mdi-magnify" :disabled="items.length == 0" v-model="search" single-line hide-details
        clearable label="Search" class="mx-4"></v-text-field>
    </v-card-title>
    <v-card-text>
      <v-data-table :headers="headers" :items="items" item-key="id" class="elevation-1" :search="search"
        :loading="loading">
        <template v-slot:top>
          <v-toolbar flat>
            <v-spacer></v-spacer>
            <v-btn color="primary" class="mb-2" nuxt :to="`${uri}create`">
              <v-icon>mdi-plus-circle-outline</v-icon>&nbsp;
              {{ $t("Create New") }}
            </v-btn>
          </v-toolbar>
        </template>
      </v-data-table>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  async fetch() {
    this.loading = true;

    const res = await this.$axios({ url: 'datasource' });
    this.items = res.data.map(x => x);

    this.loading = false;
  },
  data() {
    return {
      uri: 'datasource/',
      search: '',
      headers: [],
      items: [],
    }
  },
  mounted() {
    this.headers = [
      {
        text: this.$t('name'),
        value: 'name',
      },
      {
        text: this.$t('Type'),
        value: 'type',
      },
      { text: 'Actions', },
    ]
  },
  methods: {},
}
</script>
