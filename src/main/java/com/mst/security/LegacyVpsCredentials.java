package com.mst.security;

import java.nio.file.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import javax.crypto.Cipher;
import javax.crypto.spec.*;
import javax.xml.parsers.DocumentBuilderFactory;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Component;

/** Reads the existing desktop encrypted VPS settings; never logs or exposes credentials. */
@Component
public class LegacyVpsCredentials {
    private static final String KEY="4693024482831831",IV="4693024482831831";
    private final Path config;
    public LegacyVpsCredentials(@Value("${inventory.desktop-config:../recovered_source/bin/Debug/ECCOUNTBOOKERP.exe.config}") String path){config=Path.of(path).toAbsolutePath().normalize();}
    public Credentials read(){
        try{
            var factory=DocumentBuilderFactory.newInstance();factory.setFeature("http://apache.org/xml/features/disallow-doctype-decl",true);factory.setFeature("http://xml.org/sax/features/external-general-entities",false);factory.setFeature("http://xml.org/sax/features/external-parameter-entities",false);factory.setXIncludeAware(false);factory.setExpandEntityReferences(false);
            Map<String,String> values=new HashMap<>();try(var input=Files.newInputStream(config)){var nodes=factory.newDocumentBuilder().parse(input).getElementsByTagName("add");for(int i=0;i<nodes.getLength();i++){var node=(org.w3c.dom.Element)nodes.item(i);String name=node.getAttribute("name");if(Set.of("VPSName","VPSUser","VPSPwd").contains(name))values.put(name,decrypt(node.getAttribute("connectionString")));}}
            for(String key:List.of("VPSName","VPSUser","VPSPwd"))if(values.get(key)==null||values.get(key).isBlank())throw new IllegalStateException();
            return new Credentials(values.get("VPSName"),values.get("VPSUser"),values.get("VPSPwd"));
        }catch(Exception ex){throw new IllegalStateException("Desktop VPS configuration is unavailable or cannot be decrypted");}
    }
    private static String decrypt(String encoded) throws Exception {Cipher cipher=Cipher.getInstance("AES/CBC/PKCS5Padding");cipher.init(Cipher.DECRYPT_MODE,new SecretKeySpec(KEY.getBytes(StandardCharsets.UTF_8),"AES"),new IvParameterSpec(IV.getBytes(StandardCharsets.UTF_8)));return new String(cipher.doFinal(Base64.getDecoder().decode(encoded)),StandardCharsets.UTF_8);}
    public static final class Credentials {
        private final String server,user,password;
        public Credentials(String server,String user,String password){this.server=server;this.user=user;this.password=password;}
        public String server(){return server;}public String user(){return user;}public String password(){return password;}
        @Override public String toString(){return "Desktop VPS credentials [redacted]";}
    }
}
