﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace MassCultureLibrary.Animes
{
    internal class JsonAnimeStorage : IAnimeRepository
    {
        public JsonSerializerOptions _options = new JsonSerializerOptions() { Encoder= JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true };
        public string _filename = "anime.json";
        public List<Anime> _animes;

        public JsonAnimeStorage()
        {
            using FileStream file = new FileStream(_filename, FileMode.OpenOrCreate);
            _animes = JsonSerializer.Deserialize<List<Anime>>(file, _options) ?? new List<Anime>();
        }

        public async Task<Anime> AddAsync(Anime anime)
        {
            _animes.Add(anime);
            using FileStream file = new FileStream(_filename, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync<List<Anime>>(file, _animes, _options);
            return anime;
        }

        public async Task DeleteAsync(Guid id)
        {
            _animes.RemoveAll(x => x.Id == id);
            using FileStream file = new FileStream(_filename, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync<List<Anime>>(file, _animes, _options);
        }

        public async Task<IEnumerable<Anime>> GetAnimeAsync()
        {

            return _animes;
        }

        public async Task<Anime?> GetByIdAsync(Guid id)
        {
            var result = _animes.Find(x => x.Id == id);
            return result;
        }

        public async Task UpdateAsync(Anime anime)
        {
            var result = _animes.Find(x => x.Id == anime.Id);
            result.Title = anime.Title;
            result.Status = anime.Status;
            result.Genre = anime.Genre;
            using FileStream file = new FileStream(_filename, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync<List<Anime>>(file, _animes, _options);
        }
    }
}
