{{- define "mychart.labels1Helpers" }}
  labels:
    fullName: {{ .name }} {{ .secondName }}
    country: {{ .country }}
{{- end }}