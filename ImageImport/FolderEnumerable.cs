using MediaDevices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageImport
{
    class FolderEnumerable : IEnumerable<MediaDirectoryInfo>
    {
        private MediaDirectoryInfo Root;

        public FolderEnumerable(MediaDirectoryInfo root)
        {
            this.Root = root;
        }


        public IEnumerator<MediaDirectoryInfo> GetEnumerator()
        {
            var stack = new Stack<IEnumerator<MediaDirectoryInfo>>();
            IEnumerator<MediaDirectoryInfo> enumerator = this.Root.EnumerateDirectories().GetEnumerator();

            try
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        MediaDirectoryInfo element = enumerator.Current;
                        if (element.EnumerateFiles(".nomedia").Any())
                            continue;

                        yield return element;

                        stack.Push(enumerator);
                        enumerator = element.EnumerateDirectories().GetEnumerator();
                    }
                    else if (stack.Count > 0)
                    {
                        enumerator.Dispose();
                        enumerator = stack.Pop();
                    }
                    else
                    {
                        yield break;
                    }
                }

            }
            finally
            {
                enumerator.Dispose();
                while (stack.Count > 0)
                {
                    enumerator = stack.Pop();
                    enumerator.Dispose();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
