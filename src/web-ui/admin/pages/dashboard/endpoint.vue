<template>
  <v-row>
    <v-col>
      <v-card nuxt exact to="/datasource">
        <v-card-title>
          <v-icon>mdi-database-outline</v-icon>{{ $t('Datasources') }}
        </v-card-title>
      </v-card>
    </v-col>
    <v-col>
      <v-card nuxt exact to="/endpoint">
        <v-card-title>{{ $t('Endpoints') }}</v-card-title>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
export default {
}
</script>
