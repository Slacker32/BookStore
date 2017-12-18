using BooksShopCore.WorkWithStorage;
using BooksShopCore.WorkWithStorage.EntityStorage;
using BooksShopCore.WorkWithStorage.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BooksShopCore.WorkWithUi.LogicsSite.Preview
{
    public class Preview : IPreview
    {
        private IDataRepository<PreviewData> previewRepository;

        public Preview()
        {
            previewRepository = new GenericRepository<PreviewData>(new BookStoreContext());
        }

        public string GetPreview(int bookId)
        {
            var ret=string.Empty;
            try
            {
                var bookPreviewList = previewRepository.ReadAll().Where(p=>p.BookDataId.Equals(bookId)).ToList();
                if (bookPreviewList?.Count>0)
                {
                    var preview = bookPreviewList.First();
                    if (!string.IsNullOrEmpty(preview.Path))
                    {
                        string pathToFile = preview.Path;
                        if (File.Exists(pathToFile))
                        {
                            ret = File.ReadAllText(pathToFile);
                        }
                    }else
                    {
                        ret=preview.Data;
                    }
                }
                
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetPreview", "Ошибка получения превью из хранилища данных");
                throw;
            }
            return ret;
        }
    }
}
