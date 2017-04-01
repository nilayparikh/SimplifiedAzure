#region copyright
/*
Copyright (c) 2017 Nilay Parikh
Modifications Copyright (c) 2017 Nilay Parikh
B: https://blog.nilayparikh.com E: me@nilayparikh.com G: https://github.com/nilayparikh/

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;

namespace EasyAzure.KeyVault.Secrets
{
    public interface ISecureSecret : ISecret
    {
        SecretBundle GetSecureSecret();
        Task<SecretBundle> GetSecureSecretAsync();
    }
}