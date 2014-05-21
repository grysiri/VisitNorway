using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visit.CbisAPI.Products;

namespace Visit.CbisAPI.Products
{
	public partial class Product
	{
		public string Introduction
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.Introduction); }
		}

		public string Description
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.Description); }
		}

		public virtual string ApplicationDeadline
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.applicationdeadline); }
		}

		public string StreetAddress1
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.StreetAddress1); }
		}

		public string PostalCode
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.postalCode); }
		}

		public string CityAddress
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.cityAddress); }
		}

		public string Directions
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.direction); }
		}

		public string ExternalSignupLink
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.externalsignuplink); }
		}

		public string ExternalSignupText
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.externalsignuptext); }
		}

		public double Longitude
		{
			get { return GetAttributeValue<double>(CbisAPI.Attributes.Longitude); }
		}

		public double Latitude
		{
			get { return GetAttributeValue<double>(CbisAPI.Attributes.Latitude); }
		}

		public List<MediaObject> Images
		{
			get
			{
				Media m = GetAttributeValue<Media>(CbisAPI.Attributes.media);
				if(m == null || m.MediaList == null)
					return new List<MediaObject>();
				
				return m.MediaList.Where(ml => ml.MediaType == MediaTypes.Image).ToList();
			}
		}

		public bool MembershipRequired
		{
			get { return GetAttributeValue<bool>(CbisAPI.Attributes.membershiprequired); }
		}

		public bool SignupRequired
		{
			get { return GetAttributeValue<bool>(CbisAPI.Attributes.signuprequired); }
		}

		public string OrganizerName
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.organizerName); }
		}

		public string Other
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.other); }
		}

		public string Price
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.price); }
		}

		public string Link1Description
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.descriptionLink1); }
		}

		public string Link2Description
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.descriptionLink2); }
		}

		public string Link3Description
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.descriptionLink3); }
		}

		public string Link1Name
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.nameLink1); }
		}

		public string Link2Name
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.nameLink2); }
		}

		public string Link3Name
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.nameLink3); }
		}

		public string Link1Url
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.link); }
		}

		public string Link2Url
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.link2); }
		}

		public string Link3Url
		{
			get { return GetAttributeValue<string>(CbisAPI.Attributes.link3); }
		}

		/// <summary>
		/// Get the informationdata of a certain key
		/// </summary>
		/// <typeparam name="T">the type of the data</typeparam>
		/// <param name="data">the list of data</param>
		/// <param name="key">the key of the data</param>
		/// <param name="product"></param>
		/// <returns>data based on the given key</returns>
		public T GetAttributeValue<T>(Attributes key)
		{
			AttributeData fetch = Attributes.Where(p => p.AttributeId == ((int) key)).FirstOrDefault();
			
			if (fetch == null || fetch.Value == null)
				return default(T);

			if (fetch.Value is Value && ((Value)fetch.Value).Data != null)
			{
				return (T)((Value)fetch.Value).Data;
			}
			else if (fetch.Value is Media)
			{
				return (T)((object)fetch.Value);
			}
			else if (fetch.Value is MultiAttribute)
			{
				return (T)((object)fetch.Value);
			}

			return default(T);
		}
	}
}