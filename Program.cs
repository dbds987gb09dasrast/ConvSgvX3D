using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ConvSgvX3D
{
	class Program
	{
		//consts
		public const int X3DFORM_ID_BLENDER = 0;
		public const int X3DFORM_ID_MESHLAB = 1;

		//functions & procedures
		public static bool deleteNodeJudge(string instr, int mode)
		{
			string[] refTags = new string[]{  // available tags
				"X3D",
                "head",
                "meta",
                "Scene",
                "WorldInfo",
                "NavigationInfo",
                "Background",
                "Switch",
                "Shape",
                "Appearance",
                "Material",
                "Group",
                "Transform",
                "IndexedFaceSet"
            };

			int rtNum;
			switch (mode)
			{
				case 0:
					StringComparer comp = StringComparer.InvariantCultureIgnoreCase;
					Array.Sort(refTags, comp);
					rtNum = Array.BinarySearch(refTags, instr, comp);
					break;
				case 1:
					rtNum = Array.IndexOf(refTags, instr);
					break;
				default:
					StringComparer detec = StringComparer.InvariantCultureIgnoreCase;
					Array.Sort(refTags, detec);
					rtNum = Array.BinarySearch(refTags, instr, detec);
					break;
			}

			Console.WriteLine("rtNum: {0}", rtNum);

			if (rtNum <= -1)
			{
				return false;  //no-match
			}
			else
			{
				return true;
			}
		}

		public static bool matchNodeCL(List<XElement> elmList, string matchstring)
		{
			string[] refList = new string[elmList.Count()];
			for (int i = 0; i < elmList.Count(); i++)
			{
				refList[i] = elmList[i].Name.ToString();
				//Console.WriteLine(elmList[i].Name.ToString());
			}

			StringComparer comp = StringComparer.InvariantCultureIgnoreCase;
			Array.Sort(refList, comp);
			int rtNum = Array.BinarySearch(refList, matchstring, comp);

			if (rtNum <= -1)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public static void matchNodeWL(List<XElement> rtObjList, string matchstring, bool[] flg)
		{
			for (int h = 0; h < rtObjList.Count; h++)
			{
				Console.WriteLine("1st: " + rtObjList[h].Name.ToString());

				List<XElement> elm2nd = rtObjList[h].Elements().ToList();
				for (int i = 0; i < elm2nd.Count; i++)
				{
					Console.WriteLine("2nd: " + elm2nd[i].Name.ToString());

					List<XElement> elm3rd = elm2nd[i].Elements().ToList();
					for (int j = 0; j < elm3rd.Count; j++)
					{
						Console.WriteLine("3rd: " + elm3rd[j].Name.ToString());

						List<XElement> elm4th = elm3rd[j].Elements().ToList();
						for (int k = 0; k < elm4th.Count; k++)
						{
							Console.WriteLine("4th: " + elm4th[k].Name.ToString());

							List<XElement> elm5th = elm4th[k].Elements().ToList();
							for (int l = 0; l < elm5th.Count; l++)
							{
								Console.WriteLine("5th: " + elm5th[l].Name.ToString());

								List<XElement> elm6th = elm5th[l].Elements().ToList();
								for (int m = 0; m < elm6th.Count; m++)
								{
									Console.WriteLine("6th: " + elm6th[m].Name.ToString());

									List<XElement> elm7th = elm6th[m].Elements().ToList();
									for (int n = 0; n < elm7th.Count; n++)
									{
										Console.WriteLine("7th: " + elm7th[n].Name.ToString());

										List<XElement> elm8th = elm7th[n].Elements().ToList();
										for (int p = 0; p < elm8th.Count; p++)
										{
											Console.WriteLine("8th: " + elm8th[p].Name.ToString());

											List<XElement> elm9th = elm8th[p].Elements().ToList();
											for (int q = 0; q < elm9th.Count; q++)
											{
												Console.WriteLine("9th: " + elm9th[q].Name.ToString());

												List<XElement> elm10th = elm9th[q].Elements().ToList();
												for (int r = 0; r < elm10th.Count; r++)
												{
													Console.WriteLine("10th: " + elm10th[r].Name.ToString());
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

		}

		public static int identifyExpType(XElement rtObj)
		{
			XElement idtObj_1p;
			XElement idtObj_2p;

			//for Blender output
			try
			{
				idtObj_1p = rtObj
					.Element("Scene")
					.Element("Transform")
					.Element("Transform")
					.Element("Group")
					.Element("Shape");
			}
			catch(NullReferenceException){
				idtObj_1p = null;
			}

			//for MeshLab output
			try
			{
				idtObj_2p = rtObj
					.Element("Scene")
					.Element("Shape");
			}
			catch(NullReferenceException){
				idtObj_2p = null;
			}

			if (idtObj_1p != null)
			{
				return X3DFORM_ID_BLENDER;
			}
			else if (idtObj_2p != null)
			{
				return X3DFORM_ID_MESHLAB;
			}
			//put down switches for other formats here
			//else if(condition){
			//	return 2;
			//}
			else
			{
				return -1;  //unmatched
			}
		}

		public static XElement convertSgvX3DForm(XElement orgObj, int formId)
		{
			XElement rtObj = orgObj;
			switch (formId)
			{
				//-----------------------------------------------
				case X3DFORM_ID_BLENDER:
					/*
					//foreach (XElement elm in baseObj.Elements("Scene"))
					XElement blnForm = new XElement("Scene",
						from fs in orgObj.Element("Scene").Elements("Transform")
						select new XElement("Group",
							fs.Element("Transform").Element("Group").Attributes(),
							new XElement("Transform",
								fs.Attributes(),
								new XElement("Transform",
									fs.Element("Transform").Attributes(),
									fs.Element("Transform").Element("Group").Element("Shape")
								))));
					rtObj.Element("Scene").ReplaceAll(blnForm.Elements());
					*/

					/*
					 * foreach (XElement elm in rtObj.Element("Scene").Elements())
					 * {
					 * if (elm.Name.ToString() == "Transform")
	{
		XElement blnForm = new XElement("Group",
			elm.Element("Transform").Element("Group").Attributes(),
			new XElement("Transform",
				elm.Attributes(),
				new XElement("Transform",
					elm.Element("Transform").Attributes(),
					elm.Element("Transform").Element("Group").Element("Shape")
					)));
		rtObj.Element("Scene").Element("Transform").ReplaceAll(blnForm);
	}
}
 */

					rtObj.Element("Scene").ReplaceAll(
					from elm in rtObj.Element("Scene").Elements()
					where elm.Name == "Transform"
					select new XElement("Group",
							elm.Element("Transform").Element("Group").Attributes(),
							new XElement("Transform",
								elm.Attributes(),
								new XElement("Transform",
									elm.Element("Transform").Attributes(),
									elm.Element("Transform").Element("Group").Element("Shape")
								))));

					break;

				//-----------------------------------------------
				case X3DFORM_ID_MESHLAB:
					rtObj.Element("Scene").ReplaceAll(
						new XElement("Group",
							new XElement("Transform",
								rtObj.Element("Scene").Elements("Shape")
								)));

					break;
				//-----------------------------------------------
				default:
					rtObj = null;
					break;
			}
			return rtObj;
		}

		public static XElement loadX3Dfile(string posFile)
		{
			if (!File.Exists(posFile))
			{
				Console.WriteLine("ERR: Input file not found.");
				return null;
			}
			else
			{
				//Console.WriteLine("----------------------------------------------");
				//Console.WriteLine("SYS: Input file " + posFile + " found.");
				//Console.WriteLine("----------------------------------------------");
				XDocument tmpdoc = XDocument.Load(posFile);
				XElement rtObj = tmpdoc.Root;
				List<XElement> rtList = rtObj.Elements().ToList();
				if (rtObj.Name.ToString() != "X3D" && rtList.Count() != 1)
				{
					Console.WriteLine("ERR: Format of the specified file is NOT supported.");
					return null;
				}
				else
				{
					List<XElement> mainObj = rtObj.Elements().ToList();
					if (!matchNodeCL(mainObj, "Scene"))
					{
						Console.WriteLine("ERR: <Scene> node is missing.");
						return null;
					}
					else
					{
						Console.WriteLine("SYS: File is loaded as X3D form.");
						return rtObj;
					}
				}
			}
		}

		static void Main(string[] args)
		{
			if (args.Count() == 1)
			{
				XElement rtObj = loadX3Dfile(args[0]);
				int idt = identifyExpType(rtObj);
				XElement newObj = convertSgvX3DForm(rtObj, idt);
				//Console.WriteLine(newObj);
				string newfile = Path.GetFileNameWithoutExtension(args[0])+"_sgv.x3d";
				newObj.Save(newfile);
			}
			else
			{
				Console.WriteLine("ERR: Incorrect number of arguments.");
			}
		}
	}
}
