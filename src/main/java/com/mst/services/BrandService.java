package com.mst.services;

import com.mst.models.Brand;
import com.mst.models.UserAccount;
import com.mst.repositories.DesktopBrandRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IBrandService;
import java.util.List;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.http.HttpStatus;
import org.springframework.web.server.ResponseStatusException;

@Service("brandService")
public class BrandService implements IBrandService {
    private final DesktopBrandRepository repository;
    private final CurrentUserContext context;
    public BrandService(DesktopBrandRepository repository,CurrentUserContext context) { this.repository=repository;this.context=context; }
    @Override public List<Brand> getAll() { return repository.history(context.requireAccountingUser(),null); }
    @Override public Brand getById(int id) { return requireBrand(context.requireAccountingUser(),id); }
    private Brand requireBrand(UserAccount user,int id) {
        return repository.history(user,id).stream().findFirst().orElseThrow(()->new ResponseStatusException(HttpStatus.NOT_FOUND,"Brand not found in the current company"));
    }
    @Override @Transactional(isolation=Isolation.SERIALIZABLE)
    public Brand addOrUpdate(Brand brand) {
        UserAccount user=context.requireAccountingUser();
        String code=brand.getBrandCode()==null?"":brand.getBrandCode().trim();
        String name=brand.getBrandName()==null?"":brand.getBrandName().trim();
        if(code.isEmpty()||name.isEmpty()) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Brand Code and Brand Name are required");
        // Avoid silent truncation by the desktop procedure (name) or table (code).
        if(code.length()>50||name.length()>50) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Brand Code and Brand Name must be at most 50 characters");
        int id=brand.getId()==null?0:brand.getId();
        if(id<0) throw new ResponseStatusException(HttpStatus.BAD_REQUEST,"Invalid Brand ID");
        if(id>0) requireBrand(user,id); // Legacy update/delete procedures match ID alone.
        for(Brand existing:repository.history(user,null)) {
            if(existing.getId()!=id && (code.equalsIgnoreCase(existing.getBrandCode())||name.equalsIgnoreCase(existing.getBrandName())))
                throw new ResponseStatusException(HttpStatus.CONFLICT,"Brand Code or Brand Name already exists");
        }
        brand.setBrandCode(code);brand.setBrandName(name);
        Integer savedId=repository.save(user,brand);
        return requireBrand(user,savedId);
    }
    @Override @Transactional(isolation=Isolation.SERIALIZABLE)
    public void delete(int id) {
        UserAccount user=context.requireAccountingUser();requireBrand(user,id);repository.delete(user,id);
    }
}
