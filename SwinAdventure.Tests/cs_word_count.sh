#!/bin/zsh

cs_files=(**/*.cs)

for file in "${cs_files[@]}"; do
  word_count=$(wc -w < "$file")
  line_count=$(wc -l < "$file")
  echo "$file:\n\tWord Count: $word_count,\tLine Count: $line_count"
done
