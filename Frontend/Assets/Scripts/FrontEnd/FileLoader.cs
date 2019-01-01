using System.Collections;
using System.Collections.Generic;
using jsond;

public class FileLoader {
	public static Cards card;
	private static FileLoader instance;

	private FileLoader() {
		Jsonex j = new Jsonex();
		//j.Readjson("C:\\Users\\Administrator\\Desktop\\project\\Card\\Assets\\Plugins\\tttt.json")
		//card = j.Readjson("C:\\Users\\Administrator\\Desktop\\project\\Card\\Assets\\Plugins\\tttt.json");
		card = j.Readjson("cardLibrary.json");
	}

	public static FileLoader GetInstance() {
		if (instance == null)
			instance = new FileLoader();
		
		return instance;
	}
}
